using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.ReportElements;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Common.Reports;
using Metoda.Reporting.Common.Res;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
//using NPOI.XSSF.Streaming;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Metoda.Reporting.Excel.Reports;

public class ExcelReport : ReportBase<ISheet>
{
    public static readonly IFont BASE_FONT_11PT;

    static ExcelReport()
    {
        IWorkbook workbook = new XSSFWorkbook();

        BASE_FONT_11PT = workbook.CreateFont();
        BASE_FONT_11PT.FontHeightInPoints = 11f;
        BASE_FONT_11PT.FontName = nameof(Resource.CALIBRI);
    }

    public const int DEFAULT_WIDTH_IN_NUMBER_OF_CELLS = 5;
    protected int _documentWidthInNumberOfCells;
    public PageOrientation PageOrientation { get; set; }

    public ExcelReport(
            IList<ReportElement<ISheet>> elems,
            string title,
            IReportProgress progress,
            PageOrientation orientation)
        : base(elems, title, progress)
    {
        _documentWidthInNumberOfCells = DEFAULT_WIDTH_IN_NUMBER_OF_CELLS;
        PageOrientation = orientation;
    }

    public override Task ToFileAsync(string dest)
    {
        return Task.Run(() =>
             {
                 ToFile(dest);
             });
    }

    public override void ToFile(string dest)
    {
        try
        {
            GenFile(dest);
        }
        catch (FileNotFoundException)
        {
            throw;
        }
        catch (IOException)
        {
            throw;
        }
    }

    private void GenFile(string dest)
    {
        IWorkbook workbook = new XSSFWorkbook(); //SXSSFWorkbook();

        var title = "1";

        if (!string.IsNullOrWhiteSpace(Title))
        {
            if (Title.Length > 31)
            {
                title = Title.Substring(0, 27) + "...";
            }
            else
            {
                title = Title;
            }
        }

        ISheet sheet = workbook.CreateSheet(title);

        RenderHeader(sheet);
        Progress?.Report(0.2f);
        RenderBody(sheet);
        Progress?.Report(0.9f);
        RenderFooter(sheet);

        using FileStream stream = new(dest, FileMode.Create, FileAccess.Write);
        workbook.Write(stream, false);
    }

    protected virtual void RenderHeader(ISheet sheet)
    {
        IHeader header = sheet.Header;
        header.Left = "Metoda Finance";
        header.Right = $"Data Stampa: {DateTime.Now: dd/MM/yyyy}";
    }

    protected virtual void RenderFooter(ISheet sheet)
    {
        IFooter footer = sheet.Footer;
        footer.Right = $"Pag. {HeaderFooter.Page} di {HeaderFooter.NumPages}";
    }

    protected virtual void RenderBody(ISheet container)
    {
        container.FitToPage = true;
        float progressEndValue = 0.9f;
        float progressCurrentValue = Progress.CurrentValue;

        var tables = Elems?
            .OfType<IReportMultipleTable>()
            .Cast<IReportMultipleTable>();

        if (tables?.Any() ?? false)
        {
            int maxColCount = tables.Max(_ => _.Columns.Count);

            if (_documentWidthInNumberOfCells < maxColCount)
                _documentWidthInNumberOfCells = maxColCount;
        }

        if (!string.IsNullOrWhiteSpace(Title))
        {
            var cellStyleCenter = container.Workbook.CreateCellStyle();
            var fontBold = container.Workbook.CreateFont();
            fontBold.CloneStyleFrom(BASE_FONT_11PT);
            fontBold.IsBold = true;
            fontBold.FontHeightInPoints = 12;

            cellStyleCenter.Alignment = HorizontalAlignment.Center;
            cellStyleCenter.SetFont(fontBold);

            int lastRowIdx = container.LastRowNum;

            CellRangeAddress cellRange = new(lastRowIdx, lastRowIdx, 0, _documentWidthInNumberOfCells - 1);

            container.AddMergedRegion(cellRange);

            IRow row = container.CreateRow(lastRowIdx);

            ICell cell = row.CreateCell(0);
            cell.SetCellType(CellType.String);
            cell.SetCellValue(Title);
            cell.CellStyle = cellStyleCenter;
        }

        if (Elems != null)
        {
            float progessIncrement = 0f;
            if (Progress != null)
            {
                progessIncrement = (float)(progressEndValue - progressCurrentValue) / Elems.Count();
            }

            foreach (var elem in Elems)
            {
                elem.Render(container, _documentWidthInNumberOfCells);
                progressCurrentValue += progessIncrement;
                Progress?.Report(progressCurrentValue);

                int lastRowIdx = container.LastRowNum + 1;
                IRow newRow = container.CreateRow(lastRowIdx);
            }
        }

        Progress?.Report(progressEndValue);
    }
}