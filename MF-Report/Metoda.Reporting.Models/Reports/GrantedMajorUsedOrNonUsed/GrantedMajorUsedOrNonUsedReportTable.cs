using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using System.Collections.Generic;

namespace Metoda.Reporting.Models.Reports.GrantedMajorUsedOrNonUsed;

public class GrantedMajorUsedOrNonUsedPdfReportTable
    : PdfReportMultipleTableBase<GrantedMajorUsedOrNonUsedItem>
{
    public GrantedMajorUsedOrNonUsedPdfReportTable(
        IList<PdfTable<GrantedMajorUsedOrNonUsedItem>> tables,
        TotalRow<GrantedMajorUsedOrNonUsedItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    { 
    }
}

public class GrantedMajorUsedOrNonUsedExcelReportTable
    : ExcelReportMultipleTableBase<GrantedMajorUsedOrNonUsedItem>
{
    public GrantedMajorUsedOrNonUsedExcelReportTable(
        IList<ExcelTable<GrantedMajorUsedOrNonUsedItem>> tables,
        TotalRow<GrantedMajorUsedOrNonUsedItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}