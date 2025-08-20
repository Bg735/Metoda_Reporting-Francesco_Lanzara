using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using System.Collections.Generic;

namespace Metoda.Reporting.Models.Reports.SufferingsReportedWithOtherPhenomena;

public class SufferingsReportedWithOtherPhenomenaPdfReportTable : PdfReportMultipleTableBase<SufferingsReportedWithOtherPhenomenaItem>
{
    public SufferingsReportedWithOtherPhenomenaPdfReportTable(
        IList<PdfTable<SufferingsReportedWithOtherPhenomenaItem>> tables,
        TotalRow<SufferingsReportedWithOtherPhenomenaItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}

public class SufferingsReportedWithOtherPhenomenaExcelReportTable : ExcelReportMultipleTableBase<SufferingsReportedWithOtherPhenomenaItem>
{
    public SufferingsReportedWithOtherPhenomenaExcelReportTable(
        IList<ExcelTable<SufferingsReportedWithOtherPhenomenaItem>> tables,
        TotalRow<SufferingsReportedWithOtherPhenomenaItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}
