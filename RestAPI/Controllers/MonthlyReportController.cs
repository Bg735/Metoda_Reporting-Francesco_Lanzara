using iText.Layout;
using Metoda.Reporting.Models.Reports.MonthlyReport;
using Metoda_Report_API.Controllers.Contracts;
using Metoda_Report_Web_App___Francesco_Lanzara.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthlyReportController : FilePersistentApiController
    {
        private static readonly string reportCategory = "REPORT ANALITICO PER CONTROPARTE";

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
