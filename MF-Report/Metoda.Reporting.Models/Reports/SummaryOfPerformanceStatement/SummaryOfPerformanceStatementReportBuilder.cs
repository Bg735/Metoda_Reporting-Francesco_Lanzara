using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Pdf.Builders;

namespace Metoda.Reporting.Models.Reports.SummaryOfPerformanceStatement;

public class SummaryOfPerformanceStatementPdfReportBuilder
        : PdfReportBuilder<SummaryOfPerformanceStatementPdfReport>
{
    public SummaryOfPerformanceStatementPdfReportBuilder(
        string reportTitle = "RIEPILOGO PROSPETTO ANDAMENTALE",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait) 
        : base(reportTitle, progress, orientation)
    {
    }
}

public class SummaryOfPerformanceStatementExcelReportBuilder
        : ExcelReportBuilder<SummaryOfPerformanceStatementExcelReport>
{
    public SummaryOfPerformanceStatementExcelReportBuilder(
        string reportTitle = "RIEPILOGO PROSPETTO ANDAMENTALE",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}