using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Models.Reports.OperationalOverruns;
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
    public class OperationalOverrunsController : FilePersistentApiController
    {
        private static readonly string reportCategoryAnalitics = DocumentContent.OperationalOverrunsAnalitics.FileName;
        private static readonly string reportCategorySintetics = DocumentContent.OperationalOverrunsSintetics.FileName;

        public OperationalOverrunsController(DocumentStorageService storage, IHubContext<ReportHub> hub) : base(storage, hub)
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
                    iText.Layout.Document
                >(
                    (ReportProgress p) => new OperationalOverrunsAnaliticsPdfReportBuilder(progress: p),
                    OperationalOverrunsAnaliticsFakeData.FillBuilderByData,
                    reportCategoryAnalitics
                );
            }
            catch (Exception)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
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
                    iText.Layout.Document
                >(
                    (ReportProgress p) => new OperationalOverrunsAnaliticsExcelReportBuilder(progress: p),
                    OperationalOverrunsAnaliticsFakeData.FillBuilderByData,
                    reportCategoryAnalitics
                );
            }
            catch (Exception)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
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
                    iText.Layout.Document
                >(
                    (ReportProgress p) => new OperationalOverrunsSinteticsPdfReportBuilder(progress: p),
                    OperationalOverrunsSinteticsFakeData.FillBuilderByData,
                    reportCategorySintetics
                );
            }
            catch (Exception)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
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
                    iText.Layout.Document
                >(
                    (ReportProgress p) => new OperationalOverrunsSinteticsExcelReportBuilder(progress: p),
                    OperationalOverrunsSinteticsFakeData.FillBuilderByData,
                    reportCategorySintetics
                );
            }
            catch (Exception)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
