using iText.Layout;
using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.ReportElements;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.Reports;
using Metoda.Reporting.Pdf.Reports;
using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Metoda.Reporting.Models.Reports.Trespassing;

public class TrespassingPdfReport : PdfReport
{
    public TrespassingPdfReport(
        IList<ReportElement<Document>> elems,
        string title,
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(elems, title, progress, orientation)
    {
    }
}

public class TrespassingExcelReport : ExcelReport
{
    public TrespassingExcelReport(
        IList<ReportElement<ISheet>> elems,
        string title,
        IReportProgress progress = null,
        PageOrientation orientation = PageOrientation.Portrait)
        : base(elems, title, progress, orientation)
    {
    }
}