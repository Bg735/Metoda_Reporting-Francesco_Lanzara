using iText.Layout;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Models.Reports.MonthlyReport;
using Metoda_Report_API.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using UserDocuments.Models;
using UserDocuments.Services;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthlyReportController : FilePersistentApiController
    {
        private static readonly string reportCategory = DocumentContent.MonthlyReport.FileName;

        public MonthlyReportController(DocumentStorageService storage, IHubContext<ReportHub> hub) : base(storage, hub)
        {
        }
            
        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdf()
        {
            try
            {
                return await GenerateAndSavePdfReportAsync<
                    MonthlyReportPdfReportBuilder,
                    MonthlyPdfReport,
                    Document
                >(
                    (ReportProgress p) => new MonthlyReportPdfReportBuilder(progress: p),
                    MonthlyReportFakeData.FillBuilderByData,
                    reportCategory
                );
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("xlsm")]
        public async Task<IActionResult> GetExcel()
        {
            try
            {
                return await GenerateAndSaveExcelReportAsync<
                    MonthlyReportExcelReportBuilder,
                    MonthlyExcelReport,
                    Document
                >(
                    (ReportProgress p) => new MonthlyReportExcelReportBuilder(progress: p),
                    MonthlyReportFakeData.FillBuilderByData,
                    reportCategory
                );
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
