using iText.Layout;
using Metoda.Reporting.Models.Reports.SummaryOfPerformanceStatement;
using Metoda_Report_API.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using System.Net;
using UserDocuments.Services;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummaryOfPerformanceStatementController : FilePersistentApiController
    {
        private static readonly string reportCategory = "RIEPILOGO_PROSPETTO_ANDAMENTALE";

        public SummaryOfPerformanceStatementController(DocumentStorageService storage) : base(storage)
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
                    new SummaryOfPerformanceStatementPdfReportBuilder(),
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
                    ISheet
                >(
                    new SummaryOfPerformanceStatementExcelReportBuilder(),
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
