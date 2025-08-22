using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

// Auth: OIDC solo qui (MVC e API usano il cookie condiviso)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = "MetodaReporting";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    options.Cookie.Path = "/";
})
.AddOpenIdConnect(options =>
{
    options.Authority = "http://localhost:8080";
    options.RequireHttpsMetadata = false;
    options.MetadataAddress = "http://authserver:8080/.well-known/openid-configuration";
    options.ClientId = "mvcclient";
    options.ClientSecret = "secret_mvcclient";
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("api1");
    options.Scope.Add("offline_access");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "http://authserver:8080"
    };
    // Mantieni le sostituzioni dell’host se necessario
    options.Events = new OpenIdConnectEvents
    {
        OnRedirectToIdentityProvider = ctx =>
        {
            if (!string.IsNullOrEmpty(ctx.ProtocolMessage.IssuerAddress))
                ctx.ProtocolMessage.IssuerAddress =
                    ctx.ProtocolMessage.IssuerAddress.Replace("http://authserver:8080", "http://localhost:8080");
            return Task.CompletedTask;
        },
        OnRedirectToIdentityProviderForSignOut = ctx =>
        {
            if (!string.IsNullOrEmpty(ctx.ProtocolMessage.IssuerAddress))
                ctx.ProtocolMessage.IssuerAddress =
                    ctx.ProtocolMessage.IssuerAddress.Replace("http://authserver:8080", "http://localhost:8080");
            return Task.CompletedTask;
        }
    };
});

builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\dpkeys"))
    .SetApplicationName("MetodaReporting");

// Reverse proxy con aggiunta access token se presente (per eventuali chiamate token→API)
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(tb =>
    {
        tb.AddRequestTransform(async ctx =>
        {
            var at = await ctx.HttpContext.GetTokenAsync("access_token");
            if (!string.IsNullOrEmpty(at))
            {
                ctx.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", at);
            }
        });
    });

builder.WebHost.ConfigureKestrel(o => o.ListenAnyIP(8080));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Path esclusi da auth (handshake OIDC + discovery + favicon)
string[] authBypassPrefixes =
[
    "/connect",
    "/Identity",
    "/.well-known",
    "/signin-oidc",
    "/signout-callback-oidc",
    "/favicon.ico"
];

// Middleware di protezione “selettiva”
app.Use(async (ctx, next) =>
{
    var path = ctx.Request.Path.Value ?? "";
    if (authBypassPrefixes.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
    {
        // Nessuna challenge qui
        await next();
        return;
    }

    if (!(ctx.User?.Identity?.IsAuthenticated ?? false))
    {
        await ctx.ChallengeAsync(); // una sola challenge, niente loop
        return;
    }

    await next();
});

// Aggiungi (opzionale) favicon per evitare richieste challenge ricorsive
app.Map("/favicon.ico", builderApp =>
{
    builderApp.Run(async context =>
    {
        context.Response.StatusCode = 204;
        await context.Response.CompleteAsync();
    });
});

// Niente RequireAuthorization globale sul proxy
app.MapReverseProxy();

app.Run();