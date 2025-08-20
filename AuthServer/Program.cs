using AuthServer.Data;
using Duende.IdentityServer.Models;
using Global;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

// Cookie principale usato da ASP.NET Identity
services.ConfigureApplicationCookie(opts =>
{
    opts.Cookie.Name = ".AspNetCore.Identity.Application";
    opts.Cookie.HttpOnly = true;
    opts.Cookie.SameSite = SameSiteMode.Lax;
    opts.Cookie.SecurePolicy = CookieSecurePolicy.None; // dev su HTTP
    opts.Cookie.Path = "/";
    // NON impostare Domain (rimane host-only)
});

// Cookie usato per login esterni (se li usi)
services.ConfigureExternalCookie(opts =>
{
    opts.Cookie.SameSite = SameSiteMode.Lax;
    opts.Cookie.SecurePolicy = CookieSecurePolicy.None;
});
// Add services to the container.  
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.LoginPath = "/Account/Login";
//    options.LogoutPath = "/Account/Logout";
//    options.AccessDeniedPath = "/Account/AccessDenied";
//    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
//    options.Cookie.SameSite = SameSiteMode.Lax;
//});

// Configurazione minima IdentityServer
builder.Services.AddIdentityServer(options =>
{
    // Imposta l'IssuerUri sull'URL pubblico del gateway
    options.IssuerUri = Utils.Domain.Root;
})
    .AddInMemoryClients(
    [
        new Client
        {
            Properties = new Dictionary<string, string>{
            {"response_mode", "query"}
            },
            ClientId = "yarpclient",
            ClientSecrets = { new Duende.IdentityServer.Models.Secret("secret_client".Sha256()) },
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            RedirectUris = { Utils.Domain.UrlOf("signin-oidc") },
            PostLogoutRedirectUris = { Utils.Domain.UrlOf("signout-callback-oidc") },
            AllowedScopes = { "openid", "profile", "api1" },
            RequirePkce = true,
            AllowOfflineAccess = true
        },
        new Client
        {
            Properties = new Dictionary<string, string>{
            {"response_mode", "query"}
            },
            ClientId = "mvcclient",
            ClientSecrets = { new Duende.IdentityServer.Models.Secret("secret_mvcclient".Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { Utils.Domain.UrlOf("signin-oidc") },
            PostLogoutRedirectUris = { Utils.Domain.UrlOf("signout-callback-oidc") },
            AllowedScopes = { "openid", "profile", "api1" },
            RequirePkce = true,
            AllowOfflineAccess = true
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
    .AddAspNetIdentity<IdentityUser>();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\app\data-keys")) // Percorso persistente nel container  
    .SetApplicationName("MetodaAuthServer");

//var certPath = @"C:\app\cert.pfx";
//var certPassword = "password-certificato";
//var cert = new X509Certificate2(certPath, certPassword, X509KeyStorageFlags.MachineKeySet);
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8080);
    //serverOptions.ListenAnyIP(443, listenOptions =>
    //{
    //    try
    //    {
    //        listenOptions.UseHttps(cert);

    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Errore HTTPS: {ex.Message}");
    //        if (ex is FileNotFoundException fnfEx)
    //        {
    //            Console.WriteLine($"File non trovato: {fnfEx.FileName}");
    //        }
    //        throw;
    //    }
    //});
});

builder.Services.AddRazorPages();

var app = builder.Build();
app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
var forwardedOpts = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
};
// accetta forwarded da qualsiasi proxy (dev)
forwardedOpts.KnownNetworks.Clear();
forwardedOpts.KnownProxies.Clear();

app.UseForwardedHeaders(forwardedOpts);

// Configure the HTTP request pipeline.  
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