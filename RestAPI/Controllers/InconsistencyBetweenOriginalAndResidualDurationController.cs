using iText.Layout;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Models.Reports.InconsistencyBetweenOriginalAndResidualDuration;
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
    public class InconsistencyBetweenOriginalAndResidualDurationController : FilePersistentApiController
    {
        private static readonly string reportCategory = DocumentContent.InconsistencyBetweenOriginalAndResidualDuration.FileName;

        public InconsistencyBetweenOriginalAndResidualDurationController(DocumentStorageService storage, IHubContext<ReportHub> hub) : base(storage, hub)
        {
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdf()
        {
            try
            {
                return await GenerateAndSavePdfReportAsync<
                    InconsistencyBetweenOriginalAndResidualDurationPdfReportBuilder,
                    InconsistencyBetweenOriginalAndResidualDurationPdfReport,
                    Document
                >(
                    (ReportProgress p) => new InconsistencyBetweenOriginalAndResidualDurationPdfReportBuilder(progress: p),
                    InconsistencyBetweenOriginalAndResidualDurationFakeData.FillBuilderByData,
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
                    InconsistencyBetweenOriginalAndResidualDurationExcelReportBuilder,
                    InconsistencyBetweenOriginalAndResidualDurationExcelReport,
                    Document
                >(
                    (ReportProgress p) => new InconsistencyBetweenOriginalAndResidualDurationExcelReportBuilder(progress: p),
                    InconsistencyBetweenOriginalAndResidualDurationFakeData.FillBuilderByData,
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
