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

// Correzione: SameSitePolicy non esiste. Usare SameSiteMode (enum corretto in ASP.NET Core).
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
services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
services.AddDatabaseDeveloperPageExceptionFilter();

services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Leggi i secret da configurazione (es: appsettings.json -> "IdentityServer:Clients:yarpclient:Secret")
var yarpClientSecret = builder.Configuration["IdentityServer:Clients:yarpclient:Secret"] ?? "secret_client";
var mvcClientSecret = builder.Configuration["IdentityServer:Clients:mvcclient:Secret"] ?? "secret_mvcclient";

// Config IdentityServer (token e refresh token)
services.AddIdentityServer(options =>
{
    options.IssuerUri = Utils.Domain.Root;
})
.AddInMemoryClients(
[
    new Client
    {
        ClientId = "yarpclient",
        ClientSecrets =
        [
            new Duende.IdentityServer.Models.Secret(yarpClientSecret.Sha256())
        ],
        AllowedGrantTypes = GrantTypes.ClientCredentials,
        AllowedScopes = { "api1" },
        AccessTokenLifetime = 3600 // 60 min
    },
    new Client
    {
        ClientId = "mvcclient",
        ClientSecrets =
        [
            new Duende.IdentityServer.Models.Secret(mvcClientSecret.Sha256())
        ],
        AllowedGrantTypes = GrantTypes.Code,
        RedirectUris = { Utils.Domain.UrlOf("signin-oidc") },
        PostLogoutRedirectUris = { Utils.Domain.UrlOf("signout-callback-oidc") },
        AllowedScopes = { "openid", "profile", "api1", "offline_access" },
        RequirePkce = true,
        AllowOfflineAccess = true,
        AccessTokenLifetime = 3600,                // 60 min
        RefreshTokenExpiration = TokenExpiration.Sliding,
        SlidingRefreshTokenLifetime = 60 * 60 * 8,  // 8 ore
        AbsoluteRefreshTokenLifetime = 60 * 60 * 24,// 24 ore
        RefreshTokenUsage = TokenUsage.ReUse
    }
])
.AddInMemoryIdentityResources(
[
    new IdentityResources.OpenId(),
    new IdentityResources.Profile()
])
.AddInMemoryApiScopes(
[
    new ApiScope("api1", "API 1")
])
.AddInMemoryApiResources(
[
    new ApiResource("api1") { Scopes = { "api1" } }
])
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
app.Run();