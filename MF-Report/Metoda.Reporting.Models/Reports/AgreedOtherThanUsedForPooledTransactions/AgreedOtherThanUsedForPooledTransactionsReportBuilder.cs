using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Pdf.Builders;

namespace Metoda.Reporting.Models.Reports.AgreedOtherThanUsedForPooledTransactions;

public class AgreedOtherThanUsedForPooledTransactionsPdfReportBuilder
    : PdfReportBuilder<AgreedOtherThanUsedForPooledTransactionsPdfReport>
{
    public AgreedOtherThanUsedForPooledTransactionsPdfReportBuilder(
        string reportTitle = "ACCORDATO DIVERSO DA UTILIZZATO PER OPERAZIONI IN POOL",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait) 
        : base(reportTitle, progress, orientation)
    {
    }   
}

public class AgreedOtherThanUsedForPooledTransactionsExcelReportBuilder
    : ExcelReportBuilder<AgreedOtherThanUsedForPooledTransactionsExcelReport>
{
    public AgreedOtherThanUsedForPooledTransactionsExcelReportBuilder(
        string reportTitle = "ACCORDATO DIVERSO DA UTILIZZATO PER OPERAZIONI IN POOL",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}