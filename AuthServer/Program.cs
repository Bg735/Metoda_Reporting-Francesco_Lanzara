using AuthServer.Data;
using Duende.IdentityServer.Models;
using Global;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Data Protection: chiavi persistenti e nome app stabile
builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\dpkeys\AuthServer"))
    .SetApplicationName("AuthServer");

// Cookie policy
services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

services.ConfigureApplicationCookie(opts =>
{
    opts.Cookie.Name = ".AspNetCore.Identity.Application";
    opts.Cookie.HttpOnly = true;
    opts.Cookie.SameSite = SameSiteMode.Lax;
    opts.Cookie.SecurePolicy = CookieSecurePolicy.None;
    opts.Cookie.Path = "/";
});

services.ConfigureExternalCookie(opts =>
{
    opts.Cookie.SameSite = SameSiteMode.Lax;
    opts.Cookie.SecurePolicy = CookieSecurePolicy.None;
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Identity DB (solo Identity). Nessun accesso ai documenti/volumi
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

// HttpClient per chiamate interne a RestAPI (non esposte dal Gateway)
services.AddHttpClient("RestApiInternal", client =>
{
    client.BaseAddress = new Uri("http://restapi:8080/");
});

services.AddDatabaseDeveloperPageExceptionFilter();

// Leggi i secret da configurazione
var yarpClientSecret = builder.Configuration["IdentityServer:Clients:yarpclient:Secret"] ?? "secret_client";
var mvcClientSecret = builder.Configuration["IdentityServer:Clients:mvcclient:Secret"] ?? "secret_mvcclient";

// Config IdentityServer (token e refresh token)
services.AddIdentityServer(options =>
{
    options.IssuerUri = "http://authserver:8080";
})
.AddDeveloperSigningCredential(persistKey: true)
.AddInMemoryClients(
[
    new Client
    {
        ClientId = "yarpclient",
        ClientSecrets = [ new Duende.IdentityServer.Models.Secret(yarpClientSecret.Sha256()) ],
        AllowedGrantTypes = GrantTypes.ClientCredentials,
        AllowedScopes = { "api1" },
        AccessTokenLifetime = 3600 // 60 min
    },
    new Client
    {
        ClientId = "mvcclient",
        ClientSecrets = [ new Duende.IdentityServer.Models.Secret(mvcClientSecret.Sha256()) ],
        AllowedGrantTypes = GrantTypes.Code,
        RedirectUris = { Utils.Domain.UrlOf("signin-oidc") },
        PostLogoutRedirectUris = { Utils.Domain.UrlOf("signout-callback-oidc") },
        AllowedScopes = { "openid", "profile", "api1", "offline_access" },
        RequirePkce = true,
        AllowOfflineAccess = true,
        AccessTokenLifetime = 3600,
        RefreshTokenExpiration = TokenExpiration.Sliding,
        SlidingRefreshTokenLifetime = 60 * 60 * 8,
        AbsoluteRefreshTokenLifetime = 60 * 60 * 24,
        RefreshTokenUsage = TokenUsage.ReUse
    }
])
.AddInMemoryIdentityResources([ new IdentityResources.OpenId(), new IdentityResources.Profile() ])
.AddInMemoryApiScopes([ new ApiScope("api1", "API 1") ])
.AddInMemoryApiResources([ new ApiResource("api1") { Scopes = { "api1" } } ])
.AddAspNetIdentity<IdentityUser>();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8080);
});

services.AddRazorPages();

var app = builder.Build();
app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

var forwardedOpts = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
};
forwardedOpts.KnownNetworks.Clear();
forwardedOpts.KnownProxies.Clear();
app.UseForwardedHeaders(forwardedOpts);

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
app.UseExceptionHandler("/Error");

app.UseIdentityServer();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapRazorPages();

//using (var scope = app.Services.CreateScope())
//{
//    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
//    var docsDb = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    try
//    {
//        var migrations = docsDb.Database.GetMigrations();
//        if (migrations.Any())
//        {
//            var pending = docsDb.Database.GetPendingMigrations();
//            if (pending.Any())
//            {
//                docsDb.Database.Migrate();
//            }
//        }
//        else
//        {
//            // Nessuna migration definita in questo assembly: crea lo schema di base
//            docsDb.Database.EnsureCreated();
//        }

//        // PRAGMA consigliati per SQLite
//        docsDb.Database.ExecuteSqlRaw("PRAGMA journal_mode=WAL;");
//        docsDb.Database.ExecuteSqlRaw("PRAGMA synchronous=NORMAL;");
//        docsDb.Database.ExecuteSqlRaw("PRAGMA foreign_keys=ON;");
//        docsDb.Database.ExecuteSqlRaw("PRAGMA busy_timeout=5000;");
//    }
//    catch (Exception ex)
//    {
//        logger.LogError(ex, "Errore durante l'inizializzazione del DB dei documenti");
//        throw;
//    }
//}
app.Run();