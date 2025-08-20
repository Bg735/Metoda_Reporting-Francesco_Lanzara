using iText.Layout;
using Metoda.Reporting.Models.Reports.AgreedOtherThanUsedForPooledTransactions;
using Metoda_Report_API.Controllers.Contracts;
using Metoda_Report_Web_App___Francesco_Lanzara.Services;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using System.Net;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgreedOtherThanUsedForPooledTransactionsController : FilePersistentApiController
    {
        private static readonly string reportCategory = "ACCORDATO DIVERSO DA UTILIZZATO PER OPERAZIONI IN POOL";

        public AgreedOtherThanUsedForPooledTransactionsController(DocumentStorageService storage) : base(storage)
        {
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdf()
        {
            try
            {
                return await GenerateAndSavePdfReportAsync<
                    AgreedOtherThanUsedForPooledTransactionsPdfReportBuilder,
                    AgreedOtherThanUsedForPooledTransactionsPdfReport,
                    Document
                >(
                    new AgreedOtherThanUsedForPooledTransactionsPdfReportBuilder(),
                    AgreedOtherThanUsedForPooledTransactionsFakeData.FillBuilderByData,
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
                    AgreedOtherThanUsedForPooledTransactionsExcelReportBuilder,
                    AgreedOtherThanUsedForPooledTransactionsExcelReport,
                    ISheet
                >(
                    new AgreedOtherThanUsedForPooledTransactionsExcelReportBuilder(),
                    AgreedOtherThanUsedForPooledTransactionsFakeData.FillBuilderByData,
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
