using iText.Layout;
using Metoda.Reporting.Models.Reports.GrantedMajorUsedOrNonUsed;
using Metoda_Report_API.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserDocuments.Services;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrantedMajorUsedOrNonUsedController : FilePersistentApiController
    {
        private static readonly string reportCategory = "ACCORDATO_MAGGIORE_DI_UTILIZZATO_O_SENZA_UTILIZZATO";

        public GrantedMajorUsedOrNonUsedController(DocumentStorageService storage) : base(storage)
        {
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdf()
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

        [HttpGet("xlsm")]
        public async Task<IActionResult> GetExcel()
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
    }

}
