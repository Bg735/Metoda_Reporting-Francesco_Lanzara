using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using System.Collections.Generic;

namespace Metoda.Reporting.Models.Reports.OtherCreditsButImpaired;

public class OtherCreditsButImpairedPdfReportTable : PdfReportMultipleTableBase<OtherCreditsButImpairedItem>
{
    public OtherCreditsButImpairedPdfReportTable(
        IList<PdfTable<OtherCreditsButImpairedItem>> tables,
        TotalRow<OtherCreditsButImpairedItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}

public class OtherCreditsButImpairedExcelReportTable : ExcelReportMultipleTableBase<OtherCreditsButImpairedItem>
{
    public OtherCreditsButImpairedExcelReportTable(
        IList<ExcelTable<OtherCreditsButImpairedItem>> tables,
        TotalRow<OtherCreditsButImpairedItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}

