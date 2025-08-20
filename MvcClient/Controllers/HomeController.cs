
using Metoda_Report_Web_App___Francesco_Lanzara.Services;
using Metoda_Report_Web_App___Francesco_Lanzara.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Metoda_Report_Web_App___Francesco_Lanzara.Controllers
{
    public class HomeController : Controller
    {
        private readonly DocumentStorageService _documentService;
        private readonly HttpClient _api;

        public HomeController(DocumentStorageService documentService, IHttpClientFactory httpClientFactory)
        {
            _documentService = documentService;
            _api = httpClientFactory.CreateClient("api");
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Profile()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var email = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "";

            var recentDocs = await _documentService.ListAsync(userId, 5);

            ViewBag.Email = email;
            ViewBag.RecentDocuments = recentDocs;
            return View();
        }
        public async Task<IActionResult> CallApi(string docCategory, string format)
        {
            var response = await _api.GetAsync(Path.Combine(docCategory, format));
            var content = await response.Content.ReadAsStringAsync();
            return Content(content);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
