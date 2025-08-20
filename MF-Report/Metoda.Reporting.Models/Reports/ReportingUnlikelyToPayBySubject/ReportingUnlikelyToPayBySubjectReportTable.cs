using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using System.Collections.Generic;

namespace Metoda.Reporting.Models.Reports.ReportingUnlikelyToPayBySubject;

public class ReportingUnlikelyToPayBySubjectPdfReportTable : PdfReportMultipleTableBase<ReportingUnlikelyToPayBySubjectItem>
{
    public ReportingUnlikelyToPayBySubjectPdfReportTable(
        IList<PdfTable<ReportingUnlikelyToPayBySubjectItem>> tables,
        TotalRow<ReportingUnlikelyToPayBySubjectItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}

public class ReportingUnlikelyToPayBySubjectExcelReportTable : ExcelReportMultipleTableBase<ReportingUnlikelyToPayBySubjectItem>
{
    public ReportingUnlikelyToPayBySubjectExcelReportTable(
        IList<ExcelTable<ReportingUnlikelyToPayBySubjectItem>> tables,
        TotalRow<ReportingUnlikelyToPayBySubjectItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}