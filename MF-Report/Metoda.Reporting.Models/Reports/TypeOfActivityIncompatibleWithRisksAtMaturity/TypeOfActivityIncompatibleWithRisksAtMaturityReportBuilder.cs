using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Pdf.Builders;

namespace Metoda.Reporting.Models.Reports.TypeOfActivityIncompatibleWithRisksAtMaturity;

public class TypeOfActivityIncompatibleWithRisksAtMaturityPdfReportBuilder
    : PdfReportBuilder<TypeOfActivityIncompatibleWithRisksAtMaturityPdfReport>
{
    public TypeOfActivityIncompatibleWithRisksAtMaturityPdfReportBuilder(
        string reportTitle = "TIPO ATTIVITA’ INCOMPATIBILE CON RISCHI A SCADENZA",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}

public class TypeOfActivityIncompatibleWithRisksAtMaturityExcelReportBuilder
    : ExcelReportBuilder<TypeOfActivityIncompatibleWithRisksAtMaturityExcelReport>
{
    public TypeOfActivityIncompatibleWithRisksAtMaturityExcelReportBuilder(
        string reportTitle = "TIPO ATTIVITA’ INCOMPATIBILE CON RISCHI A SCADENZA",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}