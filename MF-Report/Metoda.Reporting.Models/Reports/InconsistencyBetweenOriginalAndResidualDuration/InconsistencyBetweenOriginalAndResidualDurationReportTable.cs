using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using System.Collections.Generic;

namespace Metoda.Reporting.Models.Reports.InconsistencyBetweenOriginalAndResidualDuration;

public class InconsistencyBetweenOriginalAndResidualDurationPdfReportTable 
    : PdfReportMultipleTableBase<InconsistencyBetweenOriginalAndResidualDurationItem>
{
    public InconsistencyBetweenOriginalAndResidualDurationPdfReportTable(
        IList<PdfTable<InconsistencyBetweenOriginalAndResidualDurationItem>> tables,
        TotalRow<InconsistencyBetweenOriginalAndResidualDurationItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}

public class InconsistencyBetweenOriginalAndResidualDurationExcelReportTable
    : ExcelReportMultipleTableBase<InconsistencyBetweenOriginalAndResidualDurationItem>
{
    public InconsistencyBetweenOriginalAndResidualDurationExcelReportTable(
        IList<ExcelTable<InconsistencyBetweenOriginalAndResidualDurationItem>> tables,
        TotalRow<InconsistencyBetweenOriginalAndResidualDurationItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}
