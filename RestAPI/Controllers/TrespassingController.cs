using iText.Layout;
using Metoda.Reporting.Models.Reports.Trespassing;
using Metoda_Report_API.Controllers.Contracts;
using Metoda_Report_Web_App___Francesco_Lanzara.Services;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using System.Net;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrespassingController : FilePersistentApiController
    {
        private static readonly string reportCategory = "SCONFINAMENTI";

        public TrespassingController(DocumentStorageService storage) : base(storage)
        {
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdfAsync()
        {
            try
            {
                return await GenerateAndSavePdfReportAsync<
                    TrespassingPdfReportBuilder,
                    TrespassingPdfReport,
                    Document
                >(
                    new TrespassingPdfReportBuilder(),
                    TrespassingFakeData.FillBuilderByData,
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
                    TrespassingExcelReportBuilder,
                    TrespassingExcelReport,
                    ISheet
                >(
                    new TrespassingExcelReportBuilder(),
                    TrespassingFakeData.FillBuilderByData,
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
