using Global;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://authserver:8080";
        options.Audience = "api1";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true
        };
        options.RequireHttpsMetadata = false;
    });
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
    //options.ListenAnyIP(443, listenOptions =>
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
    //        // Puoi anche loggare ex.ToString() per dettagli completi
    //        throw;
    //    }
    //});
});
builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\dpkeys")) // cartella condivisa
    .SetApplicationName("MetodaReporting");
// Authentication configuration
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
//})
//.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
//{
//    options.Cookie.Name = "MetodaReporting";
//    options.Cookie.HttpOnly = true;
//    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
//    options.Cookie.Path = "/";
//    options.Cookie.Domain = ".localhost"; // o il tuo dominio condiviso
//})
//.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
//{
//    var oidc = builder.Configuration.GetSection("Authentication:OpenIdConnect");
//    options.Authority = Utils.Domain.Root;
//    options.ClientId = oidc["ClientId"];
//    options.ClientSecret = oidc["ClientSecret"];
//    options.ResponseType = OpenIdConnectResponseType.Code;
//    options.SaveTokens = true;
//    options.RequireHttpsMetadata = false;
//    options.GetClaimsFromUserInfoEndpoint = true;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidIssuer = Utils.Domain.Root,
//        ValidateAudience = true,
//        ValidAudience = oidc["ClientId"],
//        ValidateLifetime = true
//    };
//})
//.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
//{
//    var jwt = builder.Configuration.GetSection("Authentication:JwtBearer");
//    options.Authority = jwt["Authority"];
//    options.Audience = jwt["Audience"];
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidIssuer = jwt["Authority"],
//        ValidateAudience = true,
//        ValidAudience = jwt["Audience"],
//        ValidateLifetime = true
//    };
//});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Authenticated", policy =>
//    {
//        policy.RequireAuthenticatedUser();
//    });
//});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(transformContext =>
    {
        transformContext.AddRequestTransform(async requestContext =>
        {
            var accessToken = requestContext.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "access_token")?.Value;

            if (!string.IsNullOrEmpty(accessToken))
            {
                requestContext.ProxyRequest.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }

            await ValueTask.CompletedTask;
        });
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.Run();