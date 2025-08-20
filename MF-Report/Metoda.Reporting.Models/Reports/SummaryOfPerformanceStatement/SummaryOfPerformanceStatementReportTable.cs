using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using System.Collections.Generic;

namespace Metoda.Reporting.Models.Reports.SummaryOfPerformanceStatement;

public class SummaryOfPerformanceStatementPdfReportTable : 
    PdfReportMultipleTableBase<SummaryOfPerformanceStatementItem>
{
    public SummaryOfPerformanceStatementPdfReportTable(
        IList<PdfTable<SummaryOfPerformanceStatementItem>> tables,
        TotalRow<SummaryOfPerformanceStatementItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}

public class SummaryOfPerformanceStatementExcelReportTable :
    ExcelReportMultipleTableBase<SummaryOfPerformanceStatementItem>
{
    public SummaryOfPerformanceStatementExcelReportTable(
        IList<ExcelTable<SummaryOfPerformanceStatementItem>> tables,
        TotalRow<SummaryOfPerformanceStatementItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}
