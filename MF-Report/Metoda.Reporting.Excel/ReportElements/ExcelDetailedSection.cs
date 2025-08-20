using Metoda.Reporting.Common.Elements.ReportElements;
using Metoda.Reporting.Excel.Reports;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Excel.ReportElements;

public class ExcelDetailedSection : SectionContainerBase<IEnumerable<ExcelDetailedSectionItem>, ISheet, ISheet>
{
    private readonly float _fontSize;
    protected readonly short _indention;

    public ExcelDetailedSection(float fontSize, short indention)
    {
        _fontSize = fontSize;
        _indention = indention;
    }

    public override void Render(ISheet container, int cols)
    {
        if (!string.IsNullOrWhiteSpace(Title))
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
            cellStyleLeft.BorderBottom = BorderStyle.Thin;

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

                container.CreateRow(container.LastRowNum + 1);
            }
        }
    }
}

