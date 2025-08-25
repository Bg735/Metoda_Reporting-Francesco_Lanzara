using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using Yarp.ReverseProxy.Transforms;
using IdentityModel.Client;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt; // new: to read JWT and check issuer

var builder = WebApplication.CreateBuilder(args);

// Nome fisso per il cookie antiforgery condiviso con MvcClient
const string AntiForgeryCookieName = "XSRF-Metoda";

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
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;

    // Cancella anche l’antiforgery al sign-out
    options.Events = new CookieAuthenticationEvents
    {
        OnSigningOut = ctx =>
        {
            ctx.HttpContext.Response.Cookies.Delete(AntiForgeryCookieName, new CookieOptions
            {
                Path = "/",
                SameSite = SameSiteMode.Lax,
                HttpOnly = false
            });
            return Task.CompletedTask;
        }
    };
})
.AddOpenIdConnect(options =>
{
    // Usa sempre l'authority del container per discovery/issuer
    options.Authority = "http://authserver:8080";
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
    options.Events = new OpenIdConnectEvents
    {
        // UI redirects devono andare su localhost
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

// YARP: aggiunge Authorization: Bearer con l’access token
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

// Path pubblici
string[] authBypassPrefixes =
[
    "/connect",
    "/Identity",
    "/.well-known",
    "/signin-oidc",
    "/signout-callback-oidc",
    "/favicon.ico",
    "/logout",
    "/css","/js","/lib","/images","/_content","/AuthServer.styles.css"
];

// Helper: richieste “interattive” (navigazione)
static bool IsInteractiveRequest(HttpContext ctx)
{
    if (!ctx.Request.Headers.TryGetValue("Accept", out var accept)) return false;
    return accept.Any(a => a.Contains("text/html", StringComparison.OrdinalIgnoreCase));
}

// Refresh automatico dell’access token (robustezza su expires_at e issuer non allineato)
app.Use(async (ctx, next) =>
{
    var path = ctx.Request.Path.Value ?? string.Empty;
    if (authBypassPrefixes.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
    {
        await next();
        return;
    }

    if (ctx.User?.Identity?.IsAuthenticated == true)
    {
        var expiresAtRaw = await ctx.GetTokenAsync("expires_at");

        // Prova a parsare in modo deterministico (ISO 8601 "o"); se manca/non parsabile, forza il refresh
        var needsRefresh = false;
        if (!DateTimeOffset.TryParseExact(expiresAtRaw, "o", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var expUtc))
        {
            needsRefresh = true;
        }
        else
        {
            needsRefresh = expUtc <= DateTimeOffset.UtcNow.AddMinutes(1);
        }

        // Nuovo: forza refresh se l'issuer del token non è quello atteso (residui di una configurazione precedente)
        var at = await ctx.GetTokenAsync("access_token");
        if (!string.IsNullOrEmpty(at))
        {
            try
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(at);
                var iss = jwt.Issuer;
                if (!string.Equals(iss, "http://authserver:8080", StringComparison.OrdinalIgnoreCase))
                {
                    needsRefresh = true;
                }
            }
            catch
            {
                // token malformato -> tenta refresh
                needsRefresh = true;
            }
        }

        if (needsRefresh)
        {
            var refreshToken = await ctx.GetTokenAsync("refresh_token");
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var discoClient = new HttpClient();
                var disco = await discoClient.GetDiscoveryDocumentAsync("http://authserver:8080");
                if (!disco.IsError)
                {
                    var tokenClient = new HttpClient();
                    var tokenResult = await tokenClient.RequestRefreshTokenAsync(new RefreshTokenRequest
                    {
                        Address = disco.TokenEndpoint,
                        ClientId = "mvcclient",
                        ClientSecret = "secret_mvcclient",
                        RefreshToken = refreshToken
                    });

                    if (!tokenResult.IsError)
                    {
                        var authResult = await ctx.AuthenticateAsync();
                        var properties = authResult?.Properties ?? new AuthenticationProperties();
                        properties.UpdateTokenValue("access_token", tokenResult.AccessToken);
                        properties.UpdateTokenValue("refresh_token", tokenResult.RefreshToken ?? refreshToken);
                        properties.UpdateTokenValue("expires_at", DateTimeOffset.UtcNow.AddSeconds(tokenResult.ExpiresIn).ToString("o", CultureInfo.InvariantCulture));
                        await ctx.SignInAsync(authResult!.Principal!, properties);
                    }
                    else
                    {
                        // Refresh fallito: pulizia antiforgery + logout + challenge/401
                        ctx.Response.Cookies.Delete(AntiForgeryCookieName, new CookieOptions
                        {
                            Path = "/",
                            SameSite = SameSiteMode.Lax,
                            HttpOnly = false
                        });
                        await ctx.SignOutAsync();

                        if (IsInteractiveRequest(ctx))
                            await ctx.ChallengeAsync(new AuthenticationProperties { RedirectUri = ctx.Request.Path + ctx.Request.QueryString });
                        else
                            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
                else
                {
                    // Discovery fallita -> come refresh fallito
                    ctx.Response.Cookies.Delete(AntiForgeryCookieName, new CookieOptions
                    {
                        Path = "/",
                        SameSite = SameSiteMode.Lax,
                        HttpOnly = false
                    });
                    await ctx.SignOutAsync();

                    if (IsInteractiveRequest(ctx))
                        await ctx.ChallengeAsync(new AuthenticationProperties { RedirectUri = ctx.Request.Path + ctx.Request.QueryString });
                    else
                        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }
            else
            {
                // Niente refresh token: pulizia antiforgery + forza login/challenge o 401
                ctx.Response.Cookies.Delete(AntiForgeryCookieName, new CookieOptions
                {
                    Path = "/",
                    SameSite = SameSiteMode.Lax,
                    HttpOnly = false
                });
                await ctx.SignOutAsync();

                if (IsInteractiveRequest(ctx))
                    await ctx.ChallengeAsync(new AuthenticationProperties { RedirectUri = ctx.Request.Path + ctx.Request.QueryString });
                else
                    ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }
    }

    await next();
});

// Protezione selettiva: se non autenticato, una sola challenge
app.Use(async (ctx, next) =>
{
    var path = ctx.Request.Path.Value ?? "";
    if (authBypassPrefixes.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
    {
        await next();
        return;
    }

    if (!(ctx.User?.Identity?.IsAuthenticated ?? false))
    {
        if (IsInteractiveRequest(ctx))
            await ctx.ChallengeAsync(new AuthenticationProperties { RedirectUri = ctx.Request.Path + ctx.Request.QueryString });
        else
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return;
    }

    await next();
});

// Intercetta 401 dalle API e forza login (utile come fallback)
app.UseStatusCodePages(async sc =>
{
    var http = sc.HttpContext;
    var path = http.Request.Path.Value ?? "";
    if (http.Response.StatusCode == 401 && !authBypassPrefixes.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
    {
        http.Response.Cookies.Delete(AntiForgeryCookieName, new CookieOptions
        {
            Path = "/",
            SameSite = SameSiteMode.Lax,
            HttpOnly = false
        });
        await http.SignOutAsync();

        if (IsInteractiveRequest(http))
            await http.ChallengeAsync(new AuthenticationProperties { RedirectUri = http.Request.Path + http.Request.QueryString });
    }
});

// Favicon
app.Map("/favicon.ico", builderApp =>
{
    builderApp.Run(async context =>
    {
        context.Response.StatusCode = 204;
        await context.Response.CompleteAsync();
    });
});

// Endpoint di logout centralizzato sul Gateway: accetta GET e POST
app.MapMethods("/logout", new[] { "GET", "POST" }, async context =>
{
    var returnUrl = context.Request.Query["returnUrl"].ToString();
    if (string.IsNullOrWhiteSpace(returnUrl))
        returnUrl = "/";

    context.Response.Cookies.Delete(AntiForgeryCookieName, new CookieOptions
    {
        Path = "/",
        SameSite = SameSiteMode.Lax,
        HttpOnly = false
    });

    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = returnUrl
    });
});

// Reverse proxy
app.MapReverseProxy();

app.Run();