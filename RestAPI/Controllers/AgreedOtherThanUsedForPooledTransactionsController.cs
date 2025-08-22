using iText.Layout;
using Metoda.Reporting.Models.Reports.AgreedOtherThanUsedForPooledTransactions;
using Metoda_Report_API.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using System.Net;
using UserDocuments.Services;

namespace Metoda_Report_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgreedOtherThanUsedForPooledTransactionsController : FilePersistentApiController
    {
        private static readonly string reportCategory = "ACCORDATO_DIVERSO_DA_UTILIZZATO_PER_OPERAZIONI_IN_POOL";

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
