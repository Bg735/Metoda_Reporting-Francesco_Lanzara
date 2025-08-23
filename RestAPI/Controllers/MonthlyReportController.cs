using iText.Layout;
using Metoda.Reporting.Models.Reports.MonthlyReport;
using Metoda_Report_API.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
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

        public MonthlyReportController(DocumentStorageService storage) : base(storage)
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
                    new MonthlyReportPdfReportBuilder(),
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
                    new MonthlyReportExcelReportBuilder(),
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
