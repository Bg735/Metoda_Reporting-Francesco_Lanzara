using iText.Layout;
using Metoda.Reporting.Models.Reports.GuaranteesConnectedWithOperationsOfACommercialNature;
using Metoda_Report_API.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserDocuments.Services;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuaranteesConnectedWithOperationsOfACommercialNatureController : FilePersistentApiController
    {
        private static readonly string reportCategory = "GARANZIE_CONNESSE_CON_OPERAZIONI_DI_NATURA_COMMERCIALE";

        public GuaranteesConnectedWithOperationsOfACommercialNatureController(DocumentStorageService storage) : base(storage)
        {
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdf()
        {
            return await GenerateAndSavePdfReportAsync<
                GuaranteesConnectedWithOperationsOfACommercialNaturePdfReportBuilder,
                GuaranteesConnectedWithOperationsOfACommercialNaturePdfReport,
                Document
            >(
                new GuaranteesConnectedWithOperationsOfACommercialNaturePdfReportBuilder(),
                GuaranteesConnectedWithOperationsOfACommercialNatureFakeData.FillBuilderByData,
                reportCategory
            );
        }

        [HttpGet("xlsm")]
        public async Task<IActionResult> GetExcel()
        {
            try
            {
                return await GenerateAndSaveExcelReportAsync<
                    GuaranteesConnectedWithOperationsOfACommercialNatureExcelReportBuilder,
                    GuaranteesConnectedWithOperationsOfACommercialNatureExcelReport,
                    Document
                >(
                    new GuaranteesConnectedWithOperationsOfACommercialNatureExcelReportBuilder(),
                    GuaranteesConnectedWithOperationsOfACommercialNatureFakeData.FillBuilderByData,
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
