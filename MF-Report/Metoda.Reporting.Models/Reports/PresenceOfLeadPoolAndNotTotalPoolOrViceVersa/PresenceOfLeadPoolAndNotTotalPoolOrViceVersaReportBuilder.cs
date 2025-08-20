using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Pdf.Builders;

namespace Metoda.Reporting.Models.Reports.PresenceOfLeadPoolAndNotTotalPoolOrViceVersa;

public class PresenceOfLeadPoolAndNotTotalPoolOrViceVersaPdfReportBuilder
        : PdfReportBuilder<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaPdfReport>
{
    public PresenceOfLeadPoolAndNotTotalPoolOrViceVersaPdfReportBuilder(
        string reportTitle = "PRESENZA DI POOL CAPOFILA E NON POOL TOTALE O VICEVERSA",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait) 
        : base(reportTitle, progress, orientation)
    {
    }
}

public class PresenceOfLeadPoolAndNotTotalPoolOrViceVersaExcelReportBuilder
        : ExcelReportBuilder<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaExcelReport>
{
    public PresenceOfLeadPoolAndNotTotalPoolOrViceVersaExcelReportBuilder(
        string reportTitle = "PRESENZA DI POOL CAPOFILA E NON POOL TOTALE O VICEVERSA",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}