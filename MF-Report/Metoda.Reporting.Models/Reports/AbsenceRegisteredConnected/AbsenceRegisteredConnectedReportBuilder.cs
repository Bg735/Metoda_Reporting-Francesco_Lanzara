using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Pdf.Builders;

namespace Metoda.Reporting.Models.Reports.AbsenceRegisteredConnected;

public class AbsenceRegisteredConnectedPdfReportBuilder
    : PdfReportBuilder<AbsenceRegisteredConnectedPdfReport>
{
    public AbsenceRegisteredConnectedPdfReportBuilder(
        string reportTitle = "ASSENZA CENSITO COLLEGATO",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait) 
        : base(reportTitle, progress, orientation)
    {
    }
}

public class AbsenceRegisteredConnectedExcelReportBuilder
    : ExcelReportBuilder<AbsenceRegisteredConnectedExcelReport>
{
    public AbsenceRegisteredConnectedExcelReportBuilder(
        string reportTitle = "ASSENZA CENSITO COLLEGATO",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait) 
        : base(reportTitle, progress, orientation)
    {
    }
}