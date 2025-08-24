using iText.Layout;
using Metoda.Reporting.Common.Builders;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Excel.Reports;
using Metoda.Reporting.Pdf.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Net;
using System.Security.Claims;
using UserDocuments.Services;

namespace Metoda_Report_API.Controllers.Contracts
{
    public abstract class FilePersistentApiController : ControllerBase
    {
        readonly private string dataFolder = Path.Combine(AppContext.BaseDirectory, "Data");
        readonly private DocumentStorageService _storage;
        readonly protected IHubContext<ReportHub> _hub;

        public FilePersistentApiController(DocumentStorageService storage, IHubContext<ReportHub> hub)
        {
            _storage = storage;
            _hub = hub;
        }

        private bool TryGetUserId(out Guid userId)
        {
            userId = default;
            var claim = User?.FindFirst(ClaimTypes.NameIdentifier) ?? User?.FindFirst("sub");
            return claim != null && Guid.TryParse(claim.Value, out userId);
        }

        private static async Task<FileStream> OpenReadWithRetryAsync(string path, int maxAttempts = 10, int initialDelayMs = 50, CancellationToken ct = default)
        {
            var delay = initialDelayMs;
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    // Read-only, consentendo condivisione per ridurre conflitti transitori
                    return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                }
                catch (IOException) when (attempt < maxAttempts)
                {
                    await Task.Delay(delay, ct);
                    delay = Math.Min(delay * 2, 1000);
                }
            }

            // ultimo tentativo: lascia propagare l'eccezione se fallisce
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
        }
        async protected Task<IActionResult> GenerateAndSavePdfReportAsync<TBuilder, TReport, TContainer>(Func<ReportProgress, TBuilder> GetBuilder, Action<TBuilder> FillBuilder, string reportType)
            where TBuilder : ReportBuilderBase<TReport, Document>
            where TReport : PdfReport
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();
            var builder = GetBuilder(
                new ReportProgress(async (value) =>
                {
                    await _hub.Clients.All.SendAsync("ReportProgress", value);
                })
            );
            FillBuilder(builder);
            var report = builder.Build();

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            string fileName = $"{reportType}|{DateTime.Now:yyyy-MM-dd__HH_mm_ss}.pdf";

            string filePath = await _storage.SaveAsync(
                userId,
                fileName,
                "application/pdf",
                report.ToFile
            );

            // Ricava l'ID (GUID) dal percorso salvato: è il nome file senza estensione
            var generatedId = Path.GetFileNameWithoutExtension(filePath) ?? string.Empty;
            await _hub.Clients.All.SendAsync("ReportCompleted", new { id = generatedId, name = Path.GetFileName(fileName) });

            try
            {
                var stream = await OpenReadWithRetryAsync(filePath);
                return File(stream, "application/pdf", fileName);
            }
            catch (IOException)
            {
                // Lock persistente: segnala come 409 o 503 per retry client
                return StatusCode((int)HttpStatusCode.Conflict, "Il file è temporaneamente non disponibile. Riprovare.");
            }
        }

        protected async Task<IActionResult> GenerateAndSaveExcelReportAsync<TBuilder, TReport, TContainer>(Func<ReportProgress, TBuilder> GetBuilder, Action<TBuilder> FillBuilder, string reportType)
            where TBuilder : ExcelReportBuilder<TReport>
            where TReport : ExcelReport
            where TContainer : class
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();
            var builder = GetBuilder(
                new ReportProgress(async (value) =>
                {
                    await _hub.Clients.All.SendAsync("ReportProgress", value);
                })
            );
            FillBuilder(builder);
            var report = builder.Build();

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            // Evita spazi e usa 24h
            string fileName = $"{reportType}_{DateTime.Now:yyyy-MM-dd__HH_mm_ss}.xlsm";

            string filePath = await _storage.SaveAsync(
                userId,
                fileName,
                // MIME ufficiale per .xlsm
                "application/vnd.ms-excel.sheet.macroEnabled.12",
                report.ToFile
            );

            var generatedId = Path.GetFileNameWithoutExtension(filePath) ?? string.Empty;
            await _hub.Clients.All.SendAsync("ReportCompleted", new { id = generatedId, name = Path.GetFileName(fileName) });

            try
            {
                var stream = await OpenReadWithRetryAsync(filePath);
                return File(stream, "application/vnd.ms-excel.sheet.macroEnabled.12", fileName);
            }
            catch (IOException)
            {
                return StatusCode((int)HttpStatusCode.Conflict, "Il file è temporaneamente non disponibile. Riprovare.");
            }
        }
    }
}
