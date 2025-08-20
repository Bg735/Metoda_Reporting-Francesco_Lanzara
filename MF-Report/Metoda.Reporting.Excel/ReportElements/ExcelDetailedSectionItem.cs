using Metoda.Reporting.Common.Elements.ReportElements;
using Metoda.Reporting.Excel.Helpers;
using Metoda.Reporting.Excel.Reports;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.ComponentModel;
using System.Linq;

namespace Metoda.Reporting.Excel.ReportElements;

public class ExcelDetailedSectionItem : DetailedSectionItemBase<ISheet, ISheet>
{
    private readonly float _fontSize;
    protected readonly short _indention;

    public ExcelDetailedSectionItem(float fontSize, short indention)
    {
        _fontSize = fontSize;
        _indention = indention;
    }

    public override void Render(ISheet container, int cols)
    {
        if (!string.IsNullOrWhiteSpace(Title))
        {
            //   .SetBorder(new SolidBorder(0.5f));
            IFont fontRgular = container.Workbook.CreateFont();
            fontRgular.CloneStyleFrom(ExcelReport.BASE_FONT_11PT);
            fontRgular.FontHeightInPoints = _fontSize;

            IFont fontBold = container.Workbook.CreateFont();
            fontBold.CloneStyleFrom(fontRgular);
            fontBold.IsBold = true;

            var cellStyleLeftBold = container.Workbook.CreateCellStyle();
            cellStyleLeftBold.Alignment = HorizontalAlignment.Left;
            cellStyleLeftBold.VerticalAlignment = VerticalAlignment.Top;
            cellStyleLeftBold.Indention = _indention;
            cellStyleLeftBold.SetFont(fontBold);

            var cellStyleLeftRegular = container.Workbook.CreateCellStyle();
            cellStyleLeftRegular.CloneStyleFrom(cellStyleLeftBold);
            cellStyleLeftRegular.SetFont(fontRgular);

            new CellWithRichTextValue(
                container,
                cols,
                fontRgular,
                fontBold,
                cellStyleLeftBold,
                Title,
                TitleEnd
                ).AddToContainer();


            int keyMergedColsCount = (int)Math.Ceiling(cols * 0.3);
            int keyMergedColsLastIdx = keyMergedColsCount - 1;
            //int valueMergedColsCount = cols - keyMergedColsCount;
            int valueMergedColsLastIdx = cols - 1;

            int lastRowIdx = container.LastRowNum + 1;

            IRow row = container.CreateRow(lastRowIdx);
            
            var cell = row.CreateCell(0);
                cell.SetCellValue(KeyHeader);
                cell.CellStyle = cellStyleLeftBold;

            var cellRange = new CellRangeAddress(lastRowIdx, lastRowIdx, 0, keyMergedColsLastIdx);
            container.AddMergedRegion(cellRange);

            cell = row.CreateCell(keyMergedColsCount);
            cell.SetCellValue(ValueHeader);
            cell.CellStyle = cellStyleLeftBold;

            cellRange = new CellRangeAddress(lastRowIdx, lastRowIdx, keyMergedColsCount, valueMergedColsLastIdx);
            container.AddMergedRegion(cellRange);

            if (Content?.Any() ?? false)
            {
                foreach (var kvp in Content)
                {
                    row = container.CreateRow(++lastRowIdx);

                    cell = row.CreateCell(0);
                    cell.SetCellValue(kvp.Key);
                    cell.CellStyle = cellStyleLeftRegular;

                    cellRange = new CellRangeAddress(lastRowIdx, lastRowIdx, 0, keyMergedColsLastIdx);
                    container.AddMergedRegion(cellRange);

                    cell = row.CreateCell(keyMergedColsCount);
                    cell.SetCellValue(kvp.Value);
                    cell.CellStyle = cellStyleLeftRegular;

                    cellRange = new CellRangeAddress(lastRowIdx, lastRowIdx, keyMergedColsCount, valueMergedColsLastIdx);
                    container.AddMergedRegion(cellRange);
                }
            }
        }
    }
}
