using iText.Layout;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Models.Reports.OtherCreditsButImpaired;
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
    public class OtherCreditsButImpairedController : FilePersistentApiController
    {
        private static readonly string reportCategory = DocumentContent.OtherCreditsButImpaired.FileName;

        public OtherCreditsButImpairedController(DocumentStorageService storage, IHubContext<ReportHub> hub) : base(storage, hub)
        {
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdf()
        {
            try
            {
                return await GenerateAndSavePdfReportAsync<
                    OtherCreditsButImpairedPdfReportBuilder,
                    OtherCreditsButImpairedPdfReport,
                    Document
                >(
                    (ReportProgress p) => new OtherCreditsButImpairedPdfReportBuilder(progress: p),
                    OtherCreditsButImpairedFakeData.FillBuilderByData,
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
                    OtherCreditsButImpairedExcelReportBuilder,
                    OtherCreditsButImpairedExcelReport,
                    Document
                >(
                    (ReportProgress p) => new OtherCreditsButImpairedExcelReportBuilder(progress: p),
                    OtherCreditsButImpairedFakeData.FillBuilderByData,
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
