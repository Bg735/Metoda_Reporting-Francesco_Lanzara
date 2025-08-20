using iText.Layout;
using Metoda.Reporting.Models.Reports.GuaranteesConnectedWithOperationsOfACommercialNature;
using Metoda_Report_API.Controllers.Contracts;
using Metoda_Report_Web_App___Francesco_Lanzara.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuaranteesConnectedWithOperationsOfACommercialNatureController : FilePersistentApiController
    {
        private static readonly string reportCategory = "GARANZIE CONNESSE CON OPERAZIONI DI NATURA COMMERCIALE";

        public GuaranteesConnectedWithOperationsOfACommercialNatureController(DocumentStorageService storage) : base(storage)
        {
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdf()
        {
            try
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
