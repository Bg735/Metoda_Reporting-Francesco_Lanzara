using Metoda.Reporting.Common.Elements.ReportElements;
using Metoda.Reporting.Excel.Reports;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Metoda.Reporting.Excel.ReportElements;

public class ExcelReportFilterSection : ReportFilterSectionBase<ISheet>
{
    public ExcelReportFilterSection() : base()
    {
    }

    public ExcelReportFilterSection(string label, string content)
        : base(label, content)
    {
    }

    public override void Render(ISheet container, int cols)
    {
        var fontBold = container.Workbook.CreateFont();
        fontBold.CloneStyleFrom(ExcelReport.BASE_FONT_11PT);
        fontBold.IsBold = true;
        
        var cellStyleLeft = container.Workbook.CreateCellStyle();
        cellStyleLeft.Alignment = HorizontalAlignment.Left;
        cellStyleLeft.VerticalAlignment = VerticalAlignment.Top;
        cellStyleLeft.SetFont(fontBold);
        cellStyleLeft.Indention = 1;

        var fontBold2 = container.Workbook.CreateFont();
        fontBold2.CloneStyleFrom(fontBold);
        fontBold2.FontHeightInPoints = fontBold.FontHeightInPoints - 1; 

        var cellStyleRight = container.Workbook.CreateCellStyle();
        cellStyleRight.CloneStyleFrom(cellStyleLeft);
        cellStyleRight.WrapText = true;
        cellStyleRight.SetFont(fontBold2);
        cellStyleRight.Indention = 0;

        int lastRowIdx = container.LastRowNum + 1;

        IRow row = container.CreateRow(lastRowIdx);

        var cell = row.CreateCell(0);
        cell.SetCellValue($"{Label?.Trim()}: ");
        cell.CellStyle = cellStyleLeft;

        cell = row.CreateCell(1);
        cell.SetCellValue(Content?.Trim());
        cell.CellStyle = cellStyleRight;

        var cellRange = new CellRangeAddress(lastRowIdx, lastRowIdx, 1, cols - 1);
        container.AddMergedRegion(cellRange);
    }
}