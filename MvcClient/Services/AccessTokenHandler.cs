namespace Metoda_Report_Web_App___Francesco_Lanzara.Services
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    public class AccessTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContext;
        public AccessTokenHandler(IHttpContextAccessor httpContext) => _httpContext = httpContext;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token)
        {
            var context = _httpContext.HttpContext;
            var access = await context.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            if (!string.IsNullOrEmpty(access))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access);
            return await base.SendAsync(request, token);
        }
    }
}
