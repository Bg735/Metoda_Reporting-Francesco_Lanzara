using iText.Layout;
using Metoda.Reporting.Models.Reports.ReportingUnlikelyToPayBySubject;
using Metoda_Report_API.Controllers.Contracts;
using Metoda_Report_Web_App___Francesco_Lanzara.Services;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using System.Net;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportingUnlikelyToPayBySubjectController : FilePersistentApiController
    {
        private static readonly string reportCategory = "SEGNALAZIONE INADEMPIENZE PROBABILI PER SOGGETTO";

        public ReportingUnlikelyToPayBySubjectController(DocumentStorageService storage) : base(storage)
        {
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdf()
        {
            try
            {
                return await GenerateAndSavePdfReportAsync<
                    ReportingUnlikelyToPayBySubjectPdfReportBuilder,
                    ReportingUnlikelyToPayBySubjectPdfReport,
                    Document
                >(
                    new ReportingUnlikelyToPayBySubjectPdfReportBuilder(),
                    ReportingUnlikelyToPayBySubjectFakeData.FillBuilderByData,
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
                    ReportingUnlikelyToPayBySubjectExcelReportBuilder,
                    ReportingUnlikelyToPayBySubjectExcelReport,
                    ISheet
                >(
                    new ReportingUnlikelyToPayBySubjectExcelReportBuilder(),
                    ReportingUnlikelyToPayBySubjectFakeData.FillBuilderByData,
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
