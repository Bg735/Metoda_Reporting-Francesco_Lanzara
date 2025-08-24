using iText.Layout;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Models.Reports.ReportingUnlikelyToPayBySubject;
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
    public class ReportingUnlikelyToPayBySubjectController : FilePersistentApiController
    {
        private static readonly string reportCategory = DocumentContent.ReportingUnlikelyToPayBySubject.FileName;

        public ReportingUnlikelyToPayBySubjectController(DocumentStorageService storage, IHubContext<ReportHub> hub) : base(storage, hub)
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
                    (ReportProgress p) => new ReportingUnlikelyToPayBySubjectPdfReportBuilder(progress: p),
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
                    Document
                >(
                    (ReportProgress p) => new ReportingUnlikelyToPayBySubjectExcelReportBuilder(progress: p),
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
