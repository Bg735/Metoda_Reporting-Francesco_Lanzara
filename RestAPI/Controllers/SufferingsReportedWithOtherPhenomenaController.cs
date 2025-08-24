using iText.Layout;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Models.Reports.SufferingsReportedWithOtherPhenomena;
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
    public class SufferingsReportedWithOtherPhenomenaController : FilePersistentApiController
    {
        private static readonly string reportCategory = DocumentContent.SufferingsReportedWithOtherPhenomena.FileName;

        public SufferingsReportedWithOtherPhenomenaController(DocumentStorageService storage, IHubContext<ReportHub> hub) : base(storage, hub)
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
                    (ReportProgress p) => new SufferingsReportedWithOtherPhenomenaPdfReportBuilder(progress: p),
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
                    Document
                >(
                    (ReportProgress p) => new SufferingsReportedWithOtherPhenomenaExcelReportBuilder(progress: p),
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
