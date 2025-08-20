using Metoda_Report_Web_App___Francesco_Lanzara.Models;
using Metoda_Report_Web_App___Francesco_Lanzara.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DocumentStorageService>();
var cs = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<UserDocsDbContext>(opt =>
    opt.UseSqlite(cs));

var authConfig = builder.Configuration.GetSection("Authentication");
var authority = authConfig["Authority"];
builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\dpkeys")) // stessa cartella
    .SetApplicationName("MetodaReporting");

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = "MetodaReporting";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;   // per redirect GET funziona in HTTP
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // dev su HTTP
    options.Cookie.Path = "/";
})
.AddOpenIdConnect(options =>
{
    options.Authority = "http://localhost:8080"; // quello che deve vedere il browser (gateway)
    options.RequireHttpsMetadata = false;

    //discovery server-side dal container authserver
    options.MetadataAddress = "http://authserver:8080/.well-known/openid-configuration";

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "http://authserver:8080" // issuer effettivo del token
    };

    options.ResponseType = OpenIdConnectResponseType.Code;
    options.ResponseMode = OpenIdConnectResponseMode.Query;
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;

    options.ClientId = "mvcclient";
    options.ClientSecret = "secret_mvcclient";
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("api1");

    // --- EVENTI: riscriviamo l'issuer/authorization endpoint solo per il redirect del browser ---
    options.Events = new OpenIdConnectEvents
    {
        OnRedirectToIdentityProvider = ctx =>
        {
            // se il metadata ha endpoint che puntano a authserver, sostituiscili col gateway
            if (!string.IsNullOrEmpty(ctx.ProtocolMessage.IssuerAddress))
            {
                ctx.ProtocolMessage.IssuerAddress =
                    ctx.ProtocolMessage.IssuerAddress.Replace("http://authserver:8080", "http://localhost:8080");
            }

            // inoltre, alcuni parametri (es. PostLogoutRedirectUri) possono essere riscritti qui se necessario
            return Task.CompletedTask;
        },

        // quando fai signout, riscrivi anche lì (se necessario)
        OnRedirectToIdentityProviderForSignOut = ctx =>
        {
            if (!string.IsNullOrEmpty(ctx.ProtocolMessage.IssuerAddress))
            {
                ctx.ProtocolMessage.IssuerAddress =
                    ctx.ProtocolMessage.IssuerAddress.Replace("http://authserver:8080", "http://localhost:8080");
            }
            return Task.CompletedTask;
        }
    };
});


builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

    var gatewayHost = "gateway"; // nome del container gateway nella rete Docker
    var addresses = Dns.GetHostAddresses(gatewayHost);

    foreach (var addr in addresses)
    {
        if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            options.KnownProxies.Add(addr);
        }
    }
});


// HttpClient per chiamate API con access token
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AccessTokenHandler>();
builder.Services.AddHttpClient("api", cli =>
{
    cli.BaseAddress = new Uri(builder.Configuration["ApiProvider:BaseUrl"]!);
})
.AddHttpMessageHandler<AccessTokenHandler>();

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
});

// MVC e Razor Pages con autorizzazione globale
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddRazorPages();

var app = builder.Build();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto
});
app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
app.UseExceptionHandler("/Home/Error");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserDocsDbContext>();
    db.Database.ExecuteSqlRaw("PRAGMA journal_mode=WAL;");
    db.Database.ExecuteSqlRaw("PRAGMA synchronous=NORMAL;");
    db.Database.ExecuteSqlRaw("PRAGMA foreign_keys=ON;");
    db.Database.ExecuteSqlRaw("PRAGMA busy_timeout=5000;");
}


app.Run();