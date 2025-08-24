using iText.Layout;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Models.Reports.PresenceOfLeadPoolAndNotTotalPoolOrViceVersa;
using Metoda_Report_API.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NPOI.SS.UserModel;
using System.Net;
using UserDocuments.Models;
using UserDocuments.Services;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresenceOfLeadPoolAndNotTotalPoolOrViceVersaController : FilePersistentApiController
    {
        private static readonly string reportCategory = DocumentContent.PresenceOfLeadPoolAndNotTotalPoolOrViceVersa.FileName;

        public PresenceOfLeadPoolAndNotTotalPoolOrViceVersaController(DocumentStorageService storage, IHubContext<ReportHub> hub) : base(storage, hub)
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
                    (ReportProgress p) => new PresenceOfLeadPoolAndNotTotalPoolOrViceVersaPdfReportBuilder(progress: p),
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
                    Document
                >(
                    (ReportProgress p) => new PresenceOfLeadPoolAndNotTotalPoolOrViceVersaExcelReportBuilder(progress: p),
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
