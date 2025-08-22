using iText.Layout;
using Metoda.Reporting.Models.Reports.InconsistencyBetweenOriginalAndResidualDuration;
using Metoda_Report_API.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserDocuments.Services;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InconsistencyBetweenOriginalAndResidualDurationController : FilePersistentApiController
    {
        private static readonly string reportCategory = "INCONGRUENZA_TRA_DURATA_ORIGINARIA_E_RESIDUA";

        public InconsistencyBetweenOriginalAndResidualDurationController(DocumentStorageService storage) : base(storage)
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
                    new InconsistencyBetweenOriginalAndResidualDurationPdfReportBuilder(),
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
                    new InconsistencyBetweenOriginalAndResidualDurationExcelReportBuilder(),
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
