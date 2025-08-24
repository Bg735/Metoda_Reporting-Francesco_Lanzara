using iText.Layout;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Models.Reports.SummaryOfPerformanceStatement;
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
    public class SummaryOfPerformanceStatementController : FilePersistentApiController
    {
        private static readonly string reportCategory = DocumentContent.SummaryOfPerformanceStatement.FileName;

        public SummaryOfPerformanceStatementController(DocumentStorageService storage, IHubContext<ReportHub> hub) : base(storage, hub)
        {
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdfAsync()
        {
            try
            {
                return await GenerateAndSavePdfReportAsync<
                    SummaryOfPerformanceStatementPdfReportBuilder,
                    SummaryOfPerformanceStatementPdfReport,
                    Document
                >(
                    (ReportProgress p) => new SummaryOfPerformanceStatementPdfReportBuilder(progress: p),
                    SummaryOfPerformanceStatementFakeData.FillBuilderByData,
                    reportCategory
                );
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("xlsm")]
        public async Task<IActionResult> GetExcelAsync()
        {
            try
            {
                return await GenerateAndSaveExcelReportAsync<
                    SummaryOfPerformanceStatementExcelReportBuilder,
                    SummaryOfPerformanceStatementExcelReport,
                    Document
                >(
                    (ReportProgress p) => new SummaryOfPerformanceStatementExcelReportBuilder(progress: p),
                    SummaryOfPerformanceStatementFakeData.FillBuilderByData,
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
