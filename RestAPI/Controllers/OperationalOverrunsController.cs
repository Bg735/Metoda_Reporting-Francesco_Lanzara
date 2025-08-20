using iText.Layout;
using Metoda.Reporting.Models.Reports.OperationalOverruns;
using Metoda_Report_API.Controllers.Contracts;
using Metoda_Report_Web_App___Francesco_Lanzara.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationalOverrunsController : FilePersistentApiController
    {
        private static readonly string reportCategoryAnalitics = "SCONFINAMENTI OPERATIVI - ANALITICO";
        private static readonly string reportCategorySintetics = "SCONFINAMENTI OPERATIVI - SINTETICO";

        public OperationalOverrunsController(DocumentStorageService storage) : base(storage)
        {
        }

        [HttpGet("Analitics/pdf")]
        public async Task<IActionResult> GetAnaliticsPdfAsync()
        {
            try
            {
                return await GenerateAndSavePdfReportAsync<
                    OperationalOverrunsAnaliticsPdfReportBuilder,
                    OperationalOverrunsAnaliticsPdfReport,
                    Document
                >(
                    new OperationalOverrunsAnaliticsPdfReportBuilder(),
                    OperationalOverrunsAnaliticsFakeData.FillBuilderByData,
                    reportCategoryAnalitics
                );
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("Analitics/xlsm")]
        public async Task<IActionResult> GetAnaliticsExcelAsync()
        {
            try
            {
                return await GenerateAndSaveExcelReportAsync<
                    OperationalOverrunsAnaliticsExcelReportBuilder,
                    OperationalOverrunsAnaliticsExcelReport,
                    Document
                >(
                    new OperationalOverrunsAnaliticsExcelReportBuilder(),
                    OperationalOverrunsAnaliticsFakeData.FillBuilderByData,
                    reportCategoryAnalitics
                );
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("Sintetics/pdf")]
        public async Task<IActionResult> GetSinteticsPdfAsync()
        {
            try
            {
                return await GenerateAndSavePdfReportAsync<
                    OperationalOverrunsSinteticsPdfReportBuilder,
                    OperationalOverrunsSinteticsPdfReport,
                    Document
                >(
                    new OperationalOverrunsSinteticsPdfReportBuilder(),
                    OperationalOverrunsSinteticsFakeData.FillBuilderByData,
                    reportCategorySintetics
                );
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("Sintetics/xlsm")]
        public async Task<IActionResult> GetSinteticsExcelAsync()
        {
            try
            {
                return await GenerateAndSaveExcelReportAsync<
                    OperationalOverrunsSinteticsExcelReportBuilder,
                    OperationalOverrunsSinteticsExcelReport,
                    Document
                >(
                    new OperationalOverrunsSinteticsExcelReportBuilder(),
                    OperationalOverrunsSinteticsFakeData.FillBuilderByData,
                    reportCategorySintetics
                );
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
