using iText.Layout;
using Metoda.Reporting.Models.Reports.OtherCreditsButImpaired;
using Metoda_Report_API.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
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

        public OtherCreditsButImpairedController(DocumentStorageService storage) : base(storage)
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
                    new OtherCreditsButImpairedPdfReportBuilder(),
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
                    ISheet
                >(
                    new OtherCreditsButImpairedExcelReportBuilder(),
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
