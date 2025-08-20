using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Pdf.Builders;

namespace Metoda.Reporting.Models.Reports.ReportingUnlikelyToPayBySubject;

public class ReportingUnlikelyToPayBySubjectPdfReportBuilder
       : PdfReportBuilder<ReportingUnlikelyToPayBySubjectPdfReport>
{
    public ReportingUnlikelyToPayBySubjectPdfReportBuilder(
        string reportTitle = "SEGNALAZIONE INADEMPIENZE PROBABILI PER SOGGETTO",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait) 
        : base(reportTitle, progress, orientation)
    {
    }
}

public class ReportingUnlikelyToPayBySubjectExcelReportBuilder
       : ExcelReportBuilder<ReportingUnlikelyToPayBySubjectExcelReport>
{
    public ReportingUnlikelyToPayBySubjectExcelReportBuilder(
        string reportTitle = "SEGNALAZIONE INADEMPIENZE PROBABILI PER SOGGETTO",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}