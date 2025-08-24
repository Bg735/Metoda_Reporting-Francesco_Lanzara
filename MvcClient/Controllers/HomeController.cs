using Metoda_Report_Web_App___Francesco_Lanzara.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Metoda_Report_Web_App___Francesco_Lanzara.Controllers
{
    public class HomeController : Controller
    {
        private const string OpenPrefCookieName = "OpenReportPreference";

        [HttpGet]
        public IActionResult Index()
        {
            bool openPref = ReadOpenPreference();
            ViewBag.OpenReportPreferred = openPref;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string docCategory, string format, bool openReport = false)
        {
            if (string.IsNullOrWhiteSpace(docCategory) || string.IsNullOrWhiteSpace(format))
                return BadRequest("Parametri mancanti.");

            // salva preferenza corrente
            SaveOpenPreference(openReport);

            var url = $"/api/{docCategory.TrimEnd('/')}/{format}";

            // Se openReport è true mantiene il comportamento corrente (redirect)
            if (openReport)
                return Redirect(url);

            // Altrimenti effettua comunque la chiamata all'API ma resta sulla Index
            // Nota: la view fa una fetch GET fire-and-forget quando openReport=false,
            // qui possiamo opzionalmente inviare una richiesta server-side. Per evitare
            // tempi di attesa e doppie chiamate, ci limitiamo a restare sulla pagina.
            TempData["ReportRequested"] = true;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetOpenPreference(bool open)
        {
            SaveOpenPreference(open);
            return Ok(new { saved = true, open });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        private string BuildCookieKey()
        {
            var rawId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            return string.IsNullOrWhiteSpace(rawId) ? OpenPrefCookieName : $"{OpenPrefCookieName}_{rawId}";
        }

        private bool ReadOpenPreference()
        {
            var key = BuildCookieKey();
            if (Request.Cookies.TryGetValue(key, out var value) && bool.TryParse(value, out var b))
                return b;
            return false; // default: non spuntata
        }

        private void SaveOpenPreference(bool open)
        {
            var key = BuildCookieKey();
            Response.Cookies.Append(key, open ? "true" : "false", new CookieOptions
            {
                HttpOnly = false, // leggibile anche client-side se necessario
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(180)
            });
        }
    }
}
