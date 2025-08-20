using iText.Layout;
using Metoda.Reporting.Common.Builders;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Excel.Reports;
using Metoda.Reporting.Pdf.Reports;
using Metoda_Report_Web_App___Francesco_Lanzara.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace Metoda_Report_API.Controllers.Contracts
{
    public abstract class FilePersistentApiController : ControllerBase
    {
        readonly private string dataFolder = Path.Combine(AppContext.BaseDirectory, "Data");
        readonly private DocumentStorageService _storage;
        readonly private Guid _userId;

        public FilePersistentApiController(DocumentStorageService storage)
        {
            _storage=storage;
            _userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        async protected Task<IActionResult> GenerateAndSavePdfReportAsync<TBuilder, TReport, TContainer>(TBuilder builder, Action<TBuilder> fillBuilderAction, string reportType) where TBuilder : ReportBuilderBase<TReport, Document> where TReport : PdfReport
        {
            fillBuilderAction(builder);
            var report = builder.Build();
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }
            string fileName = $"{reportType}_{DateTime.Now:yyyy - MM - dd__hh_mm_ss}.pdf";
            string filePath = await _storage.SaveAsync(
                    _userId,
                    fileName,
                    "application/pdf",
                    report.ToFile
            );

            var file = File(System.IO.File.ReadAllBytes(filePath), "application/pdf", fileName);
            if (file == null)
            {
                return StatusCode((int)HttpStatusCode.Gone);
            }
            else
            {
                return file;
            }
        }

        protected async Task<IActionResult> GenerateAndSaveExcelReportAsync<TBuilder, TReport, TContainer>(TBuilder builder, Action<TBuilder> fillBuilderAction, string reportType) where TBuilder : ExcelReportBuilder<TReport> where TReport : ExcelReport where TContainer : class
        {
            fillBuilderAction(builder);
            var report = builder.Build();
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }
            string fileName = $"{reportType}_{DateTime.Now:yyyy - MM - dd__hh_mm_ss}.xlsm";
            string filePath = await _storage.SaveAsync(
                     _userId,
                     fileName,
                     "application/xlsm",
                     report.ToFile
             );
            var file = File(System.IO.File.ReadAllBytes(filePath), "application/xlsm", fileName);
            if (file == null)
            {
                return StatusCode((int)HttpStatusCode.Gone);
            }
            else
            {
                return file;
            }
        }
    }
}
