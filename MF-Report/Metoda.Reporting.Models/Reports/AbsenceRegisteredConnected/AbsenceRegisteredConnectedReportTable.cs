using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using System.Collections.Generic;

namespace Metoda.Reporting.Models.Reports.AbsenceRegisteredConnected;

public class AbsenceRegisteredConnectedPdfReportTable 
    : PdfReportMultipleTableBase<AbsenceRegisteredConnectedItem>
{
    public AbsenceRegisteredConnectedPdfReportTable(
        IList<PdfTable<AbsenceRegisteredConnectedItem>> tables,
        TotalRow<AbsenceRegisteredConnectedItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}

public class AbsenceRegisteredConnectedExcelReportTable
    : ExcelReportMultipleTableBase<AbsenceRegisteredConnectedItem>
{
    public AbsenceRegisteredConnectedExcelReportTable(
        IList<ExcelTable<AbsenceRegisteredConnectedItem>> tables,
        TotalRow<AbsenceRegisteredConnectedItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}