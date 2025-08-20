using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using System.Collections.Generic;

namespace Metoda.Reporting.Models.Reports.GuaranteesConnectedWithOperationsOfACommercialNature;

public class GuaranteesConnectedWithOperationsOfACommercialNaturePdfReportTable 
    : PdfReportMultipleTableBase<GuaranteesConnectedWithOperationsOfACommercialNatureItem>
{
    public GuaranteesConnectedWithOperationsOfACommercialNaturePdfReportTable(
        IList<PdfTable<GuaranteesConnectedWithOperationsOfACommercialNatureItem>> tables,
        TotalRow<GuaranteesConnectedWithOperationsOfACommercialNatureItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}

public class GuaranteesConnectedWithOperationsOfACommercialNatureExcelReportTable
    : ExcelReportMultipleTableBase<GuaranteesConnectedWithOperationsOfACommercialNatureItem>
{
    public GuaranteesConnectedWithOperationsOfACommercialNatureExcelReportTable(
        IList<ExcelTable<GuaranteesConnectedWithOperationsOfACommercialNatureItem>> tables,
        TotalRow<GuaranteesConnectedWithOperationsOfACommercialNatureItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}
