using iText.Layout;
using Metoda.Reporting.Models.Reports.GrantedMajorUsedOrNonUsed;
using Metoda_Report_API.Controllers.Contracts;
using Metoda_Report_Web_App___Francesco_Lanzara.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrantedMajorUsedOrNonUsedController : FilePersistentApiController
    {
        private static readonly string reportCategory = "ACCORDATO MAGGIORE DI UTILIZZATO O SENZA UTILIZZATO";

        public GrantedMajorUsedOrNonUsedController(DocumentStorageService storage) : base(storage)
        {
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdf()
        {
            try
            {
                return await GenerateAndSavePdfReportAsync<
                    GrantedMajorUsedOrNonUsedPdfReportBuilder,
                    GrantedMajorUsedOrNonUsedPdfReport,
                    Document
                >(
                    new GrantedMajorUsedOrNonUsedPdfReportBuilder(),
                    GrantedMajorUsedOrNonUsedFakeData.FillBuilderByData,
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
                    GrantedMajorUsedOrNonUsedExcelReportBuilder,
                    GrantedMajorUsedOrNonUsedExcelReport,
                    Document
                >(
                    new GrantedMajorUsedOrNonUsedExcelReportBuilder(),
                    GrantedMajorUsedOrNonUsedFakeData.FillBuilderByData,
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
