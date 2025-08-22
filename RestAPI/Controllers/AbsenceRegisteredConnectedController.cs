using iText.Layout;
using Metoda.Reporting.Models.Reports.AbsenceRegisteredConnected;
using Metoda_Report_API.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserDocuments.Services;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbsenceRegisteredConnectedController : FilePersistentApiController
    {
        private static readonly string reportCategory = "ASSENZA CENSITO COLLEGATO";

        public AbsenceRegisteredConnectedController(DocumentStorageService storage) : base(storage)
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
                    new AbsenceRegisteredConnectedPdfReportBuilder(),
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
                    new AbsenceRegisteredConnectedExcelReportBuilder(),
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
