using Metoda_Report_Web_App___Francesco_Lanzara.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Metoda_Report_Web_App___Francesco_Lanzara.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string docCategory, string format)
        {
            if (string.IsNullOrWhiteSpace(docCategory) || string.IsNullOrWhiteSpace(format))
                return BadRequest("Parametri mancanti.");

            var url = $"/api/{docCategory.TrimEnd('/')}/{format}";
            return Redirect(url);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
