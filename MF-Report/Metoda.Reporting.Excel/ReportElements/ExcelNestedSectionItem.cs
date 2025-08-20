using Metoda.Reporting.Common.Elements.ReportElements;
using Metoda.Reporting.Excel.Helpers;
using Metoda.Reporting.Excel.Reports;
using NPOI.SS.UserModel;
using System.Linq;

namespace Metoda.Reporting.Excel.ReportElements;

public abstract class ExcelNestedSectionItem
    : NestedSectionItemBase<ISheet, ISheet>
{
    protected readonly float _fontSize;
    protected readonly short _indention;

    public ExcelNestedSectionItem(float fontSize, short indention)
    {
        _fontSize = fontSize;
        _indention = indention;
    }

    public override void Render(ISheet container, int cols)
    {
        IFont fontRgular = container.Workbook.CreateFont();
        fontRgular.CloneStyleFrom(ExcelReport.BASE_FONT_11PT);
        fontRgular.FontHeightInPoints = _fontSize;

        IFont fontBold = container.Workbook.CreateFont();
        fontBold.CloneStyleFrom(fontRgular);
        fontBold.IsBold = true;

        var cellStyleLeft = container.Workbook.CreateCellStyle();
        cellStyleLeft.Alignment = HorizontalAlignment.Left;
        cellStyleLeft.VerticalAlignment = VerticalAlignment.Top;
        cellStyleLeft.Indention = _indention;

        if (!string.IsNullOrWhiteSpace(Title))
        {
            new CellWithRichTextValue
                (container,
                cols,
                fontRgular,
                fontBold,
                cellStyleLeft,
                Title,
                TitleEnd
                ).AddToContainer();
        }

        if (Content?.Any() ?? false)
        {
            float subFontSize = _fontSize - 1;

            IFont subFontRgular = container.Workbook.CreateFont();
            subFontRgular.CloneStyleFrom(fontRgular);
            subFontRgular.FontHeightInPoints = subFontSize;

            IFont subFontBold = container.Workbook.CreateFont();
            subFontBold.CloneStyleFrom(subFontRgular);
            subFontBold.IsBold = true;

            var subCellStyleLeft = container.Workbook.CreateCellStyle();
            subCellStyleLeft.CloneStyleFrom(cellStyleLeft);
            subCellStyleLeft.Indention = (short)(_indention + 1);

            var richCell = new CellWithRichTextValue(
                                                        container,
                                                        cols,
                                                        subFontRgular,
                                                        subFontBold,
                                                        subCellStyleLeft,
                                                        null,
                                                        null
                                                    );
            foreach (var item in Content)
            {
                richCell.BeginString = item.Key;
                richCell.EndString = item.Value;
                richCell.AddToContainer();
            }
        }
    }
}