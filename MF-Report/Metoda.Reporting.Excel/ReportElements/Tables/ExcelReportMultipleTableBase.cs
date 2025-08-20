using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Excel.ReportElements.Tables;

public abstract class ExcelReportMultipleTableBase<T> : ReportMultipleTableBase<T, ExcelTable<T>, ISheet>
    where T : class, IReportTableRowItem
{
    bool _hideMainTotal = false;
    string[] _columnHeadTitles = null;
    public ExcelReportMultipleTableBase(
        IList<ExcelTable<T>> tables,
        TotalRow<T> mainTotalRow,
        string title,
        IntermediateTotalLocation totalLocation,
        IReportProgress progress,
        bool hideMainTotal = false,
        string[] columnHeadTitles = null)
        : base(tables, mainTotalRow, title)
    {
        IntermediateTableTotalLocation = totalLocation;
        Progress = progress;
        _hideMainTotal = hideMainTotal;
        _columnHeadTitles = columnHeadTitles;
    }

    public override void Render(ISheet container, int cols)
    {
        if (Tables?.Count > 0)
        {
            float progress = 0.3f;
            int currentRow = 0;

            var ci = new System.Globalization.CultureInfo("it-IT");

            var subTotals = IntermediateTableTotalLocation == IntermediateTotalLocation.TableBottom
                ? new List<Tuple<TotalRow<T>, IList<ReportColumn>>>()
                : null;

            if (!string.IsNullOrWhiteSpace(Title))
            {
                var titleStyle = container.Workbook.CreateCellStyle();

                var cellStyleCenter = container.Workbook.CreateCellStyle();
                var fontBold = container.Workbook.CreateFont();
                fontBold.IsBold = true;
                fontBold.FontHeightInPoints = 11;

                cellStyleCenter.Alignment = HorizontalAlignment.Center;
                cellStyleCenter.SetFont(fontBold);

                int lastRowIdx = container.LastRowNum + 1;

                var titleRow = container.CreateRow(lastRowIdx);

                ICell cell = titleRow.CreateCell(0);
                cell.SetCellType(CellType.String);
                cell.SetCellValue(Title);
                cell.CellStyle = cellStyleCenter;

                CellRangeAddress cellRange = new CellRangeAddress(lastRowIdx, lastRowIdx, 0, Columns.Count - 1);
                container.AddMergedRegion(cellRange);

            }

            Tables[0].FillHeaderRow(container, _columnHeadTitles);

            var totalOfTotals = Tables.Select(_ => _.Rows?.Count ?? 0).Sum();

            Action flush = () =>
            {
                var updatedProgress = progress + (currentRow * 0.35f) / totalOfTotals;
                if (updatedProgress < 1.0f)

                Progress?.Report(updatedProgress);
            };

            foreach (var item in Tables)
            {
                var isRenderTotal = IntermediateTableTotalLocation == IntermediateTotalLocation.TableBody || Tables.Count == 1;

                item.FillSectionByData(container, flush, ci, ref currentRow, isRenderTotal);

                if (!isRenderTotal)
                {
                    subTotals.Add(new Tuple<TotalRow<T>, IList<ReportColumn>>(item.TotalRow, item.Columns));
                }
            }

            if (Tables.Count > 1)
            {
                if (subTotals?.Any() ?? false)
                {
                    foreach (var item in subTotals)
                    {
                        ExcelTable<T>.AddTotalToTable(container, item.Item1, item.Item2, ci);
                    }
                }
            }

            if (!_hideMainTotal)
                ExcelTable<T>.AddTotalToTable(container, MainTotalRow, Tables[0].Columns, ci);
        }
        else
        {
            IRow row = container.CreateRow(container.LastRowNum);
            row.CreateCell(0).SetCellValue("NO DATA TO DISPLAY");
        }
    }
}