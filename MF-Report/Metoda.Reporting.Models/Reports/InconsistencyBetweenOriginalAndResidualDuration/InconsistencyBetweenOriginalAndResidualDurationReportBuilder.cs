using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.Builders;
using Metoda.Reporting.Pdf.Builders;

namespace Metoda.Reporting.Models.Reports.InconsistencyBetweenOriginalAndResidualDuration;

public class InconsistencyBetweenOriginalAndResidualDurationPdfReportBuilder
      : PdfReportBuilder<InconsistencyBetweenOriginalAndResidualDurationPdfReport>
{
    public InconsistencyBetweenOriginalAndResidualDurationPdfReportBuilder(
        string reportTitle = "INCONGRUENZA TRA DURATA ORIGINARIA E RESIDUA",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait) 
        : base(reportTitle, progress, orientation)
    {
    }
}

public class InconsistencyBetweenOriginalAndResidualDurationExcelReportBuilder
      : ExcelReportBuilder<InconsistencyBetweenOriginalAndResidualDurationExcelReport>
{
    public InconsistencyBetweenOriginalAndResidualDurationExcelReportBuilder(
        string reportTitle = "INCONGRUENZA TRA DURATA ORIGINARIA E RESIDUA",
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(reportTitle, progress, orientation)
    {
    }
}