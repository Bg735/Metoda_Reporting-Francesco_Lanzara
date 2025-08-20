using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Metoda.Reporting.Excel.Helpers;

public class CellWithRichTextValue
{
    public ISheet Container { get; set; }
    public int Cols { get; set; }
    public IFont FontRgular { get; set; }
    public IFont FontBold { get; set; }
    public ICellStyle CellStyleLeft { get; set; }
    public string BeginString { get; set; }
    public string EndString { get; set; }

    public CellWithRichTextValue(ISheet container, int cols, IFont rgular, IFont bold, ICellStyle style, string begin, string end)
    {
        Container = container;
        Cols = cols;
        FontRgular = rgular;
        FontBold = bold;
        CellStyleLeft = style;
        BeginString = begin;
        EndString = end;
    }

    public void AddToContainer()
    {
        string fullTitle = !string.IsNullOrWhiteSpace(EndString)
                            ? $"{BeginString}: {EndString}"
                            : $"{BeginString}:";

        IRichTextString richText = Container
                                    .Workbook
                                    .GetCreationHelper()
                                    .CreateRichTextString(fullTitle);

        richText.ApplyFont(0, BeginString.Length, FontBold);

        if (fullTitle.Length > BeginString.Length + 1)
        {
            richText.ApplyFont(BeginString.Length + 2, fullTitle.Length - 1, FontRgular);
        }

        int lastRowIdx = Container.LastRowNum + 1;

        IRow row = Container.CreateRow(lastRowIdx);

        var cell = row.CreateCell(0);
        cell.SetCellValue(richText);
        cell.CellStyle = CellStyleLeft;

        var cellRange = new CellRangeAddress(lastRowIdx, lastRowIdx, 0, Cols - 1);
        Container.AddMergedRegion(cellRange);
    }
}
