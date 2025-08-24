using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using UserDocuments.Models;
using UserDocuments.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DocumentStorageService>();

var cs = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<UserDocsDbContext>(opt => opt.UseSqlite(cs));

builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\dpkeys"))
    .SetApplicationName("MetodaReporting");

// Nome fisso del cookie antiforgery (allineato al Gateway)
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "XSRF-Metoda";
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.HttpOnly = false; // default antiforgery
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "MetodaReporting";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        options.Cookie.Path = "/";
    });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(8080));

builder.Services.AddControllersWithViews(o =>
{
    var policy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
    o.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto
});
app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();

// Rigenera il cookie antiforgery ad ogni GET autenticata
var antiforgery = app.Services.GetRequiredService<IAntiforgery>();
app.Use(async (ctx, next) =>
{
    if (HttpMethods.IsGet(ctx.Request.Method) && (ctx.User?.Identity?.IsAuthenticated ?? false))
    {
        antiforgery.GetAndStoreTokens(ctx);
    }
    await next();
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Init DB metadati documenti: migra se mancano le tabelle; crea altrimenti
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var db = scope.ServiceProvider.GetRequiredService<UserDocsDbContext>();
    try
    {
        var migrations = db.Database.GetMigrations();
        if (migrations.Any())
        {
            var pending = db.Database.GetPendingMigrations();
            if (pending.Any())
            {
                db.Database.Migrate();
            }
        }
        else
        {
            db.Database.EnsureCreated();
        }

        db.Database.ExecuteSqlRaw("PRAGMA journal_mode=WAL;");
        db.Database.ExecuteSqlRaw("PRAGMA synchronous=NORMAL;");
        db.Database.ExecuteSqlRaw("PRAGMA foreign_keys=ON;");
        db.Database.ExecuteSqlRaw("PRAGMA busy_timeout=5000;");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Errore durante l'inizializzazione del DB dei documenti");
        throw;
    }
}

app.Run();