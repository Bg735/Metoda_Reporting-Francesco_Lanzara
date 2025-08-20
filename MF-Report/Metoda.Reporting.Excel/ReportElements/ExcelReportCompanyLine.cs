using Metoda.Reporting.Common.Elements.ReportElements;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;

namespace Metoda.Reporting.Excel.ReportElements;

public class ExcelReportCompanyLine : ReportCompanyLineBase<ISheet>
{
    public ExcelReportCompanyLine(string companyName, DateTime referenceDate) : base(companyName, referenceDate) { }

    public override void Render(ISheet container, int cols)
    {
        var fontBold = container.Workbook.CreateFont();
        fontBold.IsBold = true;
        fontBold.FontHeightInPoints = 11;

        var cellStyleLeft = container.Workbook.CreateCellStyle();
        cellStyleLeft.Alignment = HorizontalAlignment.Left;
        cellStyleLeft.SetFont(fontBold);

        var cellStyleRight = container.Workbook.CreateCellStyle();
        cellStyleRight.CloneStyleFrom(cellStyleLeft);
        cellStyleRight.Alignment = HorizontalAlignment.Right;

        int lastRowIdx = container.LastRowNum + 1;

        IRow row = container.CreateRow(lastRowIdx);

        ICell cell = row.CreateCell(0);
        cell.SetCellValue(_companyName);
        cell.CellStyle = cellStyleLeft;

        var cellRange = new CellRangeAddress(lastRowIdx, lastRowIdx, 0, 1);
        container.AddMergedRegion(cellRange);

        cell = row.CreateCell(cols - 2);
        cell.SetCellValue($"Data Riferimento: {_referenceDate:dd/MM/yyyy}");
        cell.CellStyle = cellStyleRight;

        cellRange = new CellRangeAddress(lastRowIdx, lastRowIdx, cols - 2, cols - 1);
        container.AddMergedRegion(cellRange);
    }
}
