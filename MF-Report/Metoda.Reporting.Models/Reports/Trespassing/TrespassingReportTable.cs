using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using System.Collections.Generic;

namespace Metoda.Reporting.Models.Reports.Trespassing;

public class TrespassingPdfReportTable : PdfReportMultipleTableBase<TrespassingItem>
{
    public TrespassingPdfReportTable(
        IList<PdfTable<TrespassingItem>> tables,
        TotalRow<TrespassingItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}

public class TrespassingExcelReportTable : ExcelReportMultipleTableBase<TrespassingItem>
{
    public TrespassingExcelReportTable(
        IList<ExcelTable<TrespassingItem>> tables,
        TotalRow<TrespassingItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}