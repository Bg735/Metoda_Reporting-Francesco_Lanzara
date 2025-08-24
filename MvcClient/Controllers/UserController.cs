using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserDocuments.Services;

namespace Metoda_Report_Web_App___Francesco_Lanzara.Controllers
{
    public class UserController : Controller
    {
        private readonly DocumentStorageService _documentService;

        public UserController(DocumentStorageService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var rawId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (!Guid.TryParse(rawId, out var userId))
                return Challenge();

            var email = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? string.Empty;

            var docs = await _documentService.ListAsync(userId, 5);

            var tz = TimeZoneInfo.Local;
            var vm = docs.Select(d =>
            {
                var utc = DateTime.SpecifyKind(d.CreatedAtUtc, DateTimeKind.Utc);
                var local = TimeZoneInfo.ConvertTimeFromUtc(utc, tz);
                return new { Id = d.Id, FileName = d.FileNameOriginal, MimeType = d.MimeType, CreatedAtLocal = local, CreatedAtUtc = utc };
            }).ToList();

            ViewBag.Email = email;
            ViewBag.RecentDocuments = vm;
            ViewBag.TotalDocuments = await _documentService.CountAsync(userId);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Download(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();

            var rawId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (!Guid.TryParse(rawId, out var userId))
                return Challenge();

            var (stream, mimeType, fileName) = await _documentService.GetForDownloadAsync(userId, id);
            return File(stream, mimeType, fileName);
        }

        [HttpGet]
        public async Task<IActionResult> DocumentCount()
        {
            var rawId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (!Guid.TryParse(rawId, out var userId))
                return Unauthorized();

            var count = await _documentService.CountAsync(userId);
            return Json(new { count });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProfile()
        {
            // Ora deleghiamo la cancellazione totale alla pagina di Identity
            var deleteUrl = "/Identity/Account/Manage/DeletePersonalData";
            return Redirect(deleteUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            // logout locale cookie MVC
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("MetodaReporting");

            // reindirizza al logout centralizzato (Gateway)
            var target = $"/logout?returnUrl={Uri.EscapeDataString(returnUrl ?? Url.Action("Index", "Home")!)}";
            return RedirectPreserveMethod(target);
        }
    }
}