using Metoda.Reporting.Common.Elements.ReportElements;
using Metoda.Reporting.Excel.Reports;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Excel.ReportElements;

public abstract class ExcelNestedSectionBase<TListItem>
        : SectionContainerBase<IList<TListItem>, ISheet, ISheet>
    where TListItem : ExcelNestedSectionItem
{
    protected readonly float _fontSize;
    protected readonly short _indention;

    public ExcelNestedSectionBase(float fontSize, short indention)
    {
        _fontSize = fontSize;
        _indention = indention;
    }

    public override void Render(ISheet container, int cols)
    {
        IFont fontBold = container.Workbook.CreateFont();
        fontBold.CloneStyleFrom(ExcelReport.BASE_FONT_11PT);
        fontBold.FontHeightInPoints = _fontSize;
        fontBold.IsBold = true;

        var cellStyleLeft = container.Workbook.CreateCellStyle();
        cellStyleLeft.Alignment = HorizontalAlignment.Left;
        cellStyleLeft.VerticalAlignment = VerticalAlignment.Top;
        cellStyleLeft.Indention = _indention;
        cellStyleLeft.SetFont(fontBold);

        if (!string.IsNullOrWhiteSpace(Title))
        {
            int lastRowIdx = container.LastRowNum + 1;

            IRow row = container.CreateRow(lastRowIdx);

            var cell = row.CreateCell(0);
            cell.SetCellValue(Title);
            cell.CellStyle = cellStyleLeft;

            var cellRange = new CellRangeAddress(lastRowIdx, lastRowIdx, 0, cols - 1);
            container.AddMergedRegion(cellRange);
        }

        if (Content?.Any() ?? false)
        {
            foreach (var item in Content)
            {
                item.Render(container, cols);
            }
        }
    }
}