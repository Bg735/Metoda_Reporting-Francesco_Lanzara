using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Pdf.Builders;

namespace Metoda.Reporting.Models.Reports.MonthlyReport;

public class MonthlyReportPdfReportBuilder : PdfReportBuilder<MonthlyPdfReport>
{
    public MonthlyReportPdfReportBuilder(
        string reportTitle = "REPORT ANALITICO PER CONTROPARTE",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait) 
        : base(reportTitle, progress, orientation)
    {
    }
}

public class MonthlyReportExcelReportBuilder : ExcelReportBuilder<MonthlyExcelReport>
{
    public MonthlyReportExcelReportBuilder(
        string reportTitle = "REPORT ANALITICO PER CONTROPARTE",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}