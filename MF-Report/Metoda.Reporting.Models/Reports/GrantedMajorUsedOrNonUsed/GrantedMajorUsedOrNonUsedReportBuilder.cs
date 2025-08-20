using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Pdf.Builders;

namespace Metoda.Reporting.Models.Reports.GrantedMajorUsedOrNonUsed;

public class GrantedMajorUsedOrNonUsedPdfReportBuilder
    : PdfReportBuilder<GrantedMajorUsedOrNonUsedPdfReport>
{
    public GrantedMajorUsedOrNonUsedPdfReportBuilder(
        string reportTitle = "ACCORDATO MAGGIORE DI UTILIZZATO O SENZA UTILIZZATO",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}

public class GrantedMajorUsedOrNonUsedExcelReportBuilder
    : ExcelReportBuilder<GrantedMajorUsedOrNonUsedExcelReport>
{
    public GrantedMajorUsedOrNonUsedExcelReportBuilder(
        string reportTitle = "ACCORDATO MAGGIORE DI UTILIZZATO O SENZA UTILIZZATO",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}