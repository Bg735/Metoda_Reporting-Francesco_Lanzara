using iText.Layout;
using Metoda.Reporting.Models.Reports.TypeOfActivityIncompatibleWithRisksAtMaturity;
using Metoda_Report_API.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using System.Net;
using UserDocuments.Services;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeOfActivityIncompatibleWithRisksAtMaturityController : FilePersistentApiController
    {
        private static readonly string reportCategory = "TIPO ATTIVITA’ INCOMPATIBILE CON RISCHI A SCADENZA";

        public TypeOfActivityIncompatibleWithRisksAtMaturityController(DocumentStorageService storage) : base(storage)
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
                    new TypeOfActivityIncompatibleWithRisksAtMaturityPdfReportBuilder(),
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
                    ISheet
                >(
                    new TypeOfActivityIncompatibleWithRisksAtMaturityExcelReportBuilder(),
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
