using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using System.Collections.Generic;

namespace Metoda.Reporting.Models.Reports.PresenceOfLeadPoolAndNotTotalPoolOrViceVersa;

public class PresenceOfLeadPoolAndNotTotalPoolOrViceVersaPdfReportTable : 
    PdfReportMultipleTableBase<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem>
{
    public PresenceOfLeadPoolAndNotTotalPoolOrViceVersaPdfReportTable(
        IList<PdfTable<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem>> tables,
        TotalRow<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}

public class PresenceOfLeadPoolAndNotTotalPoolOrViceVersaExcelReportTable :
    ExcelReportMultipleTableBase<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem>
{
    public PresenceOfLeadPoolAndNotTotalPoolOrViceVersaExcelReportTable(
        IList<ExcelTable<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem>> tables,
        TotalRow<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}