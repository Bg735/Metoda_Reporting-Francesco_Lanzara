using iText.Layout;
using Metoda.Reporting.Models.Reports.PresenceOfLeadPoolAndNotTotalPoolOrViceVersa;
using Metoda_Report_API.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using System.Net;
using UserDocuments.Services;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresenceOfLeadPoolAndNotTotalPoolOrViceVersaController : FilePersistentApiController
    {
        private static readonly string reportCategory = "PRESENZA DI POOL CAPOFILA E NON POOL TOTALE O VICEVERSA";

        public PresenceOfLeadPoolAndNotTotalPoolOrViceVersaController(DocumentStorageService storage) : base(storage)
        {
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdfAsync()
        {
            try
            {
                return await GenerateAndSavePdfReportAsync<
                    PresenceOfLeadPoolAndNotTotalPoolOrViceVersaPdfReportBuilder,
                    PresenceOfLeadPoolAndNotTotalPoolOrViceVersaPdfReport,
                    Document
                >(
                    new PresenceOfLeadPoolAndNotTotalPoolOrViceVersaPdfReportBuilder(),
                    PresenceOfLeadPoolAndNotTotalPoolOrViceVersaFakeData.FillBuilderByData,
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
                    PresenceOfLeadPoolAndNotTotalPoolOrViceVersaExcelReportBuilder,
                    PresenceOfLeadPoolAndNotTotalPoolOrViceVersaExcelReport,
                    ISheet
                >(
                    new PresenceOfLeadPoolAndNotTotalPoolOrViceVersaExcelReportBuilder(),
                    PresenceOfLeadPoolAndNotTotalPoolOrViceVersaFakeData.FillBuilderByData,
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
