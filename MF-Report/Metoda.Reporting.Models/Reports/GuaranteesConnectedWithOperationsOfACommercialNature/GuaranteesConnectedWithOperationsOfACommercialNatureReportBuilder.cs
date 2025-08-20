using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Pdf.Builders;

namespace Metoda.Reporting.Models.Reports.GuaranteesConnectedWithOperationsOfACommercialNature;

public class GuaranteesConnectedWithOperationsOfACommercialNaturePdfReportBuilder
    : PdfReportBuilder<GuaranteesConnectedWithOperationsOfACommercialNaturePdfReport>
{
    public GuaranteesConnectedWithOperationsOfACommercialNaturePdfReportBuilder(
        string reportTitle = "GARANZIE CONNESSE CON OPERAZIONI DI NATURA COMMERCIALE",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait) 
        : base(reportTitle, progress, orientation)
    {
    }
}

public class GuaranteesConnectedWithOperationsOfACommercialNatureExcelReportBuilder
    : ExcelReportBuilder<GuaranteesConnectedWithOperationsOfACommercialNatureExcelReport>
{
    public GuaranteesConnectedWithOperationsOfACommercialNatureExcelReportBuilder(
        string reportTitle = "GARANZIE CONNESSE CON OPERAZIONI DI NATURA COMMERCIALE",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}