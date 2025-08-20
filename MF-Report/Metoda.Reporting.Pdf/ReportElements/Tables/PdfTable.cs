using iText.IO.Font;
using iText.Kernel.Font;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Res;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Metoda.Reporting.Pdf.ReportElements.Tables;

public class PdfTable<T> : ReportTableBase<T> 
    where T : class, IReportTableRowItem
{
    protected readonly PdfFont RegularFont = PdfFontFactory.CreateFont(Resource.CALIBRI, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
    protected readonly PdfFont BoldFont = PdfFontFactory.CreateFont(Resource.CALIBRIB, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
    protected readonly PdfFont ItalicFont = PdfFontFactory.CreateFont(Resource.CALIBRII, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

    public PdfTable(IList<T> rows, TotalRow<T> totalRow = null)
        : base(rows, totalRow)
    {
    }

    public virtual void FillSectionByData(Table table, Action flush, CultureInfo ci, ref int currentRow, bool hasTotal = true)
    {
        int batchSize = 1000; // Number of items to process in each batch
        int rowCount = 0;

        foreach (var row in Rows)
        {
            foreach (var val in row.GetValueArray(PropInfos, ci))
            {
                Cell cell = new Cell()
                    .Add(new Paragraph(val.Item2).SetMultipliedLeading(0.9f))
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetTextAlignment((TextAlignment)val.Item1);

                table.AddCell(cell);
            }

            currentRow++;
            rowCount++;

            // Flush the table periodically to prevent memory overload
            if (rowCount % batchSize == 0)
            {
                flush?.Invoke();
            }
        }

        if (hasTotal)
        {
            AddTotalToTable(table, GetTotalCells(TotalRow, Columns, ci));
        }
    }

    public static IList<Cell> GetTotalCells(TotalRow<T> totalRow, IList<ReportColumn> columns, CultureInfo ci)
    {
        if (totalRow != null)
        {
            PdfFont boldFont = PdfFontFactory.CreateFont(Resource.CALIBRIB, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

            IList<Cell> cells = new List<Cell>();

            var borderWidth = new SolidBorder(0.7f);
            var totalCols = columns.TakeWhile(c => !c.IsInTotal);

            cells.Add(new Cell(1, totalCols.Count())
                    .SetFont(boldFont)
                    .SetBorderBottom(borderWidth)
                    .SetBorderTop(borderWidth)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetPaddingRight(10f)
                    .Add(new Paragraph(totalRow.Label ?? "No Data")));

            var sumColsForTotal = columns.SkipWhile(c => !c.IsInTotal);

            foreach (var column in sumColsForTotal)
            {
                Cell cell = new();

                var value = ReportTableRowItemBase.GetValue(totalRow.Row, column.PropInfo, ci);

                if (!string.IsNullOrEmpty(value))
                {
                    cell.SetBorderBottom(borderWidth)
                    .SetBorderTop(borderWidth)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .Add(new Paragraph(value)
                        .SetFont(boldFont)
                        .SetMultipliedLeading(0.9f));
                }

                cells.Add(cell);
            }

            return cells;
        }
        else
        {
            return null;
        }
    }

    public static void AddTotalToTable(Table table, IList<Cell> cells)
    {
        if (cells != null)
        {
            foreach (var cell in cells)
            {
                table.AddCell(cell);
            }

            table.Flush();
        }
    }
}
