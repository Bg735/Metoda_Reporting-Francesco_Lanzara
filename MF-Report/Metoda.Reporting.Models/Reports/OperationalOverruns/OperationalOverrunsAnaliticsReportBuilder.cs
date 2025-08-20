using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Pdf.Builders;

namespace Metoda.Reporting.Models.Reports.OperationalOverruns;

public class OperationalOverrunsAnaliticsPdfReportBuilder
    : PdfReportBuilder<OperationalOverrunsAnaliticsPdfReport>
{
    public OperationalOverrunsAnaliticsPdfReportBuilder(
        string reportTitle = "SCONFINAMENTI OPERATIVI - ANALITICO",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}

public class OperationalOverrunsAnaliticsExcelReportBuilder
    : ExcelReportBuilder<OperationalOverrunsAnaliticsExcelReport>
{
    public OperationalOverrunsAnaliticsExcelReportBuilder(
        string reportTitle = "SCONFINAMENTI OPERATIVI - ANALITICO",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}