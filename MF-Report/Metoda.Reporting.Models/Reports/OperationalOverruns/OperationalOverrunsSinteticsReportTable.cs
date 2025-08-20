using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using System.Collections.Generic;

namespace Metoda.Reporting.Models.Reports.OperationalOverruns;

#region OperationalOverrunsSintetics

public class OperationalOverrunsSinteticsPdfReportTable
    : PdfReportMultipleTableBase<OperationalOverrunsSinteticsItem>
{
    public OperationalOverrunsSinteticsPdfReportTable(
        IList<PdfTable<OperationalOverrunsSinteticsItem>> tables,
        TotalRow<OperationalOverrunsSinteticsItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}

public class OperationalOverrunsSinteticsExcelReportTable
    : ExcelReportMultipleTableBase<OperationalOverrunsSinteticsItem>
{
    public OperationalOverrunsSinteticsExcelReportTable(
        IList<ExcelTable<OperationalOverrunsSinteticsItem>> tables,
        TotalRow<OperationalOverrunsSinteticsItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}

#endregion

#region OperationalOverrunsAnalitics

public class OperationalOverrunsAnaliticsPdfReportTable : PdfReportMultipleTableBase<OperationalOverrunsAnaliticsItem>
{
    public OperationalOverrunsAnaliticsPdfReportTable(
        IList<PdfTable<OperationalOverrunsAnaliticsItem>> tables,
        TotalRow<OperationalOverrunsAnaliticsItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}

public class OperationalOverrunsAnaliticsExcelReportTable : ExcelReportMultipleTableBase<OperationalOverrunsAnaliticsItem>
{
    public OperationalOverrunsAnaliticsExcelReportTable(
        IList<ExcelTable<OperationalOverrunsAnaliticsItem>> tables,
        TotalRow<OperationalOverrunsAnaliticsItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}

#endregion