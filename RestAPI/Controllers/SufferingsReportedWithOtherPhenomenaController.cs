using iText.Layout;
using Metoda.Reporting.Models.Reports.SufferingsReportedWithOtherPhenomena;
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
    public class SufferingsReportedWithOtherPhenomenaController : FilePersistentApiController
    {
        private static readonly string reportCategory = DocumentContent.SufferingsReportedWithOtherPhenomena.FileName;

        public SufferingsReportedWithOtherPhenomenaController(DocumentStorageService storage) : base(storage)
        {
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdf()
        {
            try
            {
                return await GenerateAndSavePdfReportAsync<
                    SufferingsReportedWithOtherPhenomenaPdfReportBuilder,
                    SufferingsReportedWithOtherPhenomenaPdfReport,
                    Document
                >(
                    new SufferingsReportedWithOtherPhenomenaPdfReportBuilder(),
                    SufferingsReportedWithOtherPhenomenaFakeData.FillBuilderByData,
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
                    SufferingsReportedWithOtherPhenomenaExcelReportBuilder,
                    SufferingsReportedWithOtherPhenomenaExcelReport,
                    ISheet
                >(
                    new SufferingsReportedWithOtherPhenomenaExcelReportBuilder(),
                    SufferingsReportedWithOtherPhenomenaFakeData.FillBuilderByData,
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
