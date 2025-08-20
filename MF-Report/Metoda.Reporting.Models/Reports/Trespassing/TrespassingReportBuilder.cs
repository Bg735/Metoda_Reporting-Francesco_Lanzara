using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Pdf.Builders;

namespace Metoda.Reporting.Models.Reports.Trespassing;

public class TrespassingPdfReportBuilder
        : PdfReportBuilder<TrespassingPdfReport>
{
    public TrespassingPdfReportBuilder(
        string reportTitle = "SCONFINAMENTI",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait) 
        : base(reportTitle, progress, orientation)
    {
    }
}

public class TrespassingExcelReportBuilder
        : ExcelReportBuilder<TrespassingExcelReport>
{
    public TrespassingExcelReportBuilder(
        string reportTitle = "SCONFINAMENTI",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}