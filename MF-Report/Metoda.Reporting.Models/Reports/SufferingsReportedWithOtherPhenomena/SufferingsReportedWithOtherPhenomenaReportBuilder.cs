using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Pdf.Builders;

namespace Metoda.Reporting.Models.Reports.SufferingsReportedWithOtherPhenomena;

public class SufferingsReportedWithOtherPhenomenaPdfReportBuilder
       : PdfReportBuilder<SufferingsReportedWithOtherPhenomenaPdfReport>
{
    public SufferingsReportedWithOtherPhenomenaPdfReportBuilder(
        string reportTitle = "SOFFERENZE SEGNALATE CON ALTRI CUBI",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait) 
        : base(reportTitle, progress, orientation)
    {
    }
}

public class SufferingsReportedWithOtherPhenomenaExcelReportBuilder
       : ExcelReportBuilder<SufferingsReportedWithOtherPhenomenaExcelReport>
{
    public SufferingsReportedWithOtherPhenomenaExcelReportBuilder(
        string reportTitle = "SOFFERENZE SEGNALATE CON ALTRI CUBI",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}