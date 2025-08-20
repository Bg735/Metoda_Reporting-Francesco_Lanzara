using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Pdf.Builders;
using Metoda.Reporting.Common.Enums;

namespace Metoda.Reporting.Models.Reports.OperationalOverruns;

public class OperationalOverrunsSinteticsPdfReportBuilder
       : PdfReportBuilder<OperationalOverrunsSinteticsPdfReport>
{
    public OperationalOverrunsSinteticsPdfReportBuilder(
        string reportTitle = "SCONFINAMENTI OPERATIVI - SINTETICO",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}

public class OperationalOverrunsSinteticsExcelReportBuilder
       : ExcelReportBuilder<OperationalOverrunsSinteticsExcelReport>
{
    public OperationalOverrunsSinteticsExcelReportBuilder(
        string reportTitle = "SCONFINAMENTI OPERATIVI - SINTETICO",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}