using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Excel.Helpers;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Metoda.Reporting.Excel.ReportElements.Tables;

public class ExcelTable<T> : ReportTableBase<T> where T : class, IReportTableRowItem
{
    public ExcelTable(IList<T> rows, TotalRow<T> totalRow = null)
        : base(rows, totalRow)
    {
    }

    // Returns Header Row Index
    public virtual void FillHeaderRow(ISheet section, string[] columnHeadTitles = null)
    {
        var columns = Columns.Select(_ => _.DisplayName).ToArray();
        var colWidths = Columns.Select(_ => _.ColWidth).ToArray();
  
        var cellStyle = section.Workbook.CreateCellStyle();
        cellStyle.BorderBottom = BorderStyle.Thin;
        cellStyle.BorderLeft = BorderStyle.Thin;
        cellStyle.BorderRight = BorderStyle.Thin;
        cellStyle.BorderTop = BorderStyle.Thin;
        cellStyle.Alignment = HorizontalAlignment.Center;
        var font = cellStyle.GetFont(section.Workbook);
        font.IsBold = true;
        cellStyle.SetFont(font);

        int lastRowIdx = section.LastRowNum + 1;

        ICell cell;
        IRow row = section.CreateRow(lastRowIdx);

        for (int i = 0; i < columns.Length; i++)
        {
            cell = row.CreateCell(i);

            if (columnHeadTitles != null && columnHeadTitles.Length > i && !string.IsNullOrEmpty(columnHeadTitles[i]))
                cell.SetCellValue(columnHeadTitles[i]);
            else
                cell.SetCellValue(columns[i]);
            cell.CellStyle = cellStyle;
        }
    }

    public virtual void FillSectionByData(ISheet section, Action flush, CultureInfo ci, ref int currentRow, bool hasTotal = true)
    {
        int batchSize = 1000; // Number of items to process in each batch
        int rowCount = 0;
        IRow row;
        ICell cell;
        int tableBodyBaseLine = section.LastRowNum;

        int cellIdx = 0;

        for (int colIdx = 0; colIdx < Columns.Count; colIdx++)
        {
            int currTableBodyLine = tableBodyBaseLine;

            var cellStyle = section.Workbook.CreateCellStyle();
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.Alignment = (HorizontalAlignment)(Columns[colIdx].TextAlignment + 1);

            var orig_font = cellStyle.GetFont(section.Workbook);
            IFont font = section.Workbook.CreateFont();
            font.FontName = orig_font.FontName;
            font.FontHeightInPoints = orig_font.FontHeightInPoints;
            font.IsBold = false;
            cellStyle.SetFont(font);

            foreach (var item in Rows)
            {
                row = cellIdx > 0
                    ? section.GetRow(++currTableBodyLine)
                    : section.CreateRow(++currTableBodyLine);

                cell = row.CreateCell(cellIdx);
                cell.SetCellType(CellType.String);
                cell.SetCellValue(ReportTableRowItemBase.GetValue(item, Columns[colIdx].PropInfo, ci));
                cell.CellStyle = cellStyle;

                currentRow++;
                rowCount++;

                // Flush the table periodically to prevent memory overload
                if (rowCount % batchSize == 0)
                {
                    flush?.Invoke();
                }
            }

            cellIdx++;
        }

        if (hasTotal)
        {
            AddTotalToTable(section, TotalRow, Columns, ci);
        }
    }

    public static void AddTotalToTable(ISheet section, TotalRow<T> totalRow, IList<ReportColumn> columns, CultureInfo ci)
    {
        if (totalRow != null && section != null)
        {
            int lastRowNum = section.LastRowNum + 1;
            IRow row = section.CreateRow(lastRowNum);

            var cellStyle = section.Workbook.CreateCellStyle();

            cellStyle.Alignment = HorizontalAlignment.Right;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BottomBorderColor = IndexedColors.Black.Index;


            var font = cellStyle.GetFont(section.Workbook);
            font.IsBold = true;
            cellStyle.SetFont(font);

            var totalCols = columns.TakeWhile(c => !c.IsInTotal).ToList();

            ICell cell;
            int startCellIdx = 0;

            if (totalCols.Count > 0)
            {
                cell = row.CreateCell(startCellIdx);
                cell.SetCellType(CellType.String);
                cell.SetCellValue(totalRow.Label);
                cell.CellStyle = cellStyle;

                if (totalCols.Count > 1)
                {
                    for (int i = startCellIdx + 1; i < totalCols.Count; i++)
                    {
                        var c = cell = row.CreateCell(i);
                        c.CellStyle = cellStyle;
                    }

                    startCellIdx = totalCols.Count - 1;
                    CellRangeAddress cellRange = new(lastRowNum, lastRowNum, 0, startCellIdx);
                    section.AddMergedRegion(cellRange);
                }
            }

            var sumColsForTotal = columns.SkipWhile(c => !c.IsInTotal);

            foreach (var column in sumColsForTotal)
            {
                ICellStyle clonedStyle = section.Workbook.CreateCellStyle();
                clonedStyle.CloneStyleFrom(cellStyle);

                clonedStyle.Alignment = (HorizontalAlignment)(column.TextAlignment + 1);
                cell = row.CreateCell(++startCellIdx);
                cell.SetCellType(Utils.GetCellType(column.PropInfo.PropertyType));
                cell.SetCellValue(ReportTableRowItemBase.GetValue(totalRow.Row, column.PropInfo, ci));
                cell.CellStyle = clonedStyle;
            }
        }
    }
}