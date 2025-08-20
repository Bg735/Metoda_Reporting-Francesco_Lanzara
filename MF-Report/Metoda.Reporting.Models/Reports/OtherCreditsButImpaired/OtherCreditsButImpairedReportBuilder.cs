using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Pdf.Builders;

namespace Metoda.Reporting.Models.Reports.OtherCreditsButImpaired;

public class OtherCreditsButImpairedPdfReportBuilder
        : PdfReportBuilder<OtherCreditsButImpairedPdfReport>
{
    public OtherCreditsButImpairedPdfReportBuilder(
        string reportTitle = "ALTRI CREDITI MA DETERIORATI",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait) 
        : base(reportTitle, progress, orientation)
    {
    }
}

public class OtherCreditsButImpairedExcelReportBuilder
        : ExcelReportBuilder<OtherCreditsButImpairedExcelReport>
{
    public OtherCreditsButImpairedExcelReportBuilder(
        string reportTitle = "ALTRI CREDITI MA DETERIORATI",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}