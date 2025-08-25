using Microsoft.AspNetCore.Mvc;
using UserDocuments.Services;

namespace Metoda_Report_API.Controllers.Internal
{
    // Controller interno non esposto pubblicamente: mappato su /internal e ignorato da ApiExplorer
    [ApiController]
    [Route("internal/userdocs")] // es: POST /internal/userdocs/{userId}/delete-all
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UserDocumentsAdminController : ControllerBase
    {
        private readonly DocumentStorageService _storage;
        private readonly ILogger<UserDocumentsAdminController> _logger;

        public UserDocumentsAdminController(DocumentStorageService storage, ILogger<UserDocumentsAdminController> logger)
        {
            _storage = storage;
            _logger = logger;
        }

        [HttpPost("{userId:guid}/delete-all")]
        public async Task<IActionResult> DeleteAll(Guid userId)
        {
            var count = await _storage.DeleteAllForUserAsync(userId);
            _logger.LogInformation("Deleted {Count} documents for user {UserId}", count, userId);
            return Ok(new { deleted = count });
        }
    }
}
