using iText.Layout;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Models.Reports.AbsenceRegisteredConnected;
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
    public class AbsenceRegisteredConnectedController : FilePersistentApiController
    {
        private static readonly string reportCategory = DocumentContent.AbsenceRegisteredConnected.FileName;

        public AbsenceRegisteredConnectedController(DocumentStorageService storage, IHubContext<ReportHub> hub) : base(storage, hub)
        {
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdf()
        {
            try
            {
                return await GenerateAndSavePdfReportAsync<
                    AbsenceRegisteredConnectedPdfReportBuilder,
                    AbsenceRegisteredConnectedPdfReport,
                    Document
                >(
                    (ReportProgress p)=>new AbsenceRegisteredConnectedPdfReportBuilder(progress:p),
                    AbsenceRegisteredConnectedFakeData.FillBuilderByData,
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
                    AbsenceRegisteredConnectedExcelReportBuilder,
                    AbsenceRegisteredConnectedExcelReport,
                    Document
                >(
                    (ReportProgress p) => new AbsenceRegisteredConnectedExcelReportBuilder(progress: p),
                    AbsenceRegisteredConnectedFakeData.FillBuilderByData,
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
