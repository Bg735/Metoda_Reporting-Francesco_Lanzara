using iText.Layout;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Models.Reports.TypeOfActivityIncompatibleWithRisksAtMaturity;
using Metoda_Report_API.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using UserDocuments.Services;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeOfActivityIncompatibleWithRisksAtMaturityController : FilePersistentApiController
    {
        private static readonly string reportCategory = "TIPO_ATTIVITA’_INCOMPATIBILE_CON_RISCHI_A_SCADENZA";

        public TypeOfActivityIncompatibleWithRisksAtMaturityController(DocumentStorageService storage, IHubContext<ReportHub> hub) : base(storage, hub)
        {
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdfAsync()
        {
            try
            {
                return await GenerateAndSavePdfReportAsync<
                    TypeOfActivityIncompatibleWithRisksAtMaturityPdfReportBuilder,
                    TypeOfActivityIncompatibleWithRisksAtMaturityPdfReport,
                    Document
                >(
                    (ReportProgress p) => new TypeOfActivityIncompatibleWithRisksAtMaturityPdfReportBuilder(progress: p),
                    TypeOfActivityIncompatibleWithRisksAtMaturityFakeData.FillBuilderByData,
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
                    TypeOfActivityIncompatibleWithRisksAtMaturityExcelReportBuilder,
                    TypeOfActivityIncompatibleWithRisksAtMaturityExcelReport,
                    Document
                >(
                    (ReportProgress p) => new TypeOfActivityIncompatibleWithRisksAtMaturityExcelReportBuilder(progress: p),
                    TypeOfActivityIncompatibleWithRisksAtMaturityFakeData.FillBuilderByData,
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
