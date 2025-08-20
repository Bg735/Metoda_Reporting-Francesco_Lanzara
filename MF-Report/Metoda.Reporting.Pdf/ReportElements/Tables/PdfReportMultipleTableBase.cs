using iText.IO.Font;
using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Common.Res;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Pdf.ReportElements.Tables;

public abstract class PdfReportMultipleTableBase<T>
    : ReportMultipleTableBase<T, PdfTable<T>, Document>
    where T : class, IReportTableRowItem
{
    protected readonly PdfFont RegularFont = PdfFontFactory.CreateFont(Resource.CALIBRI, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
    protected readonly PdfFont BoldFont = PdfFontFactory.CreateFont(Resource.CALIBRIB, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
    protected readonly PdfFont ItalicFont = PdfFontFactory.CreateFont(Resource.CALIBRII, PdfEncodings.WINANSI, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

    public PdfReportMultipleTableBase(
        IList<PdfTable<T>> tables,
        TotalRow<T> mainTotalRow,
        string title,
        IntermediateTotalLocation totalLocation,
        IReportProgress progress)
        : base(tables, mainTotalRow, title, totalLocation)
    {
        Progress = progress;
    }

    private Table CreateTable()
    {
        var columns = Columns.Select(_ => _.DisplayName).ToArray();
        var colWidths = Columns.Select(_ => _.ColWidth).ToArray();
        var borderWidth = new SolidBorder(0.7f);

        Table table = new Table(UnitValue.CreatePercentArray(colWidths), true)
           .UseAllAvailableWidth()
           .SetBorder(Border.NO_BORDER)// borderWidth)
           .SetMarginTop(1.0f)
           .SetMarginBottom(1.0f)
           .SetFontSize(8.0f)
           .SetTextAlignment(TextAlignment.CENTER)
           .SetVerticalAlignment(VerticalAlignment.MIDDLE)
           .SetFont(RegularFont);

        #region Table Headers 
        Cell cell;

        for (int i = 0; i < columns.Length; i++)
        {
            cell = new Cell()
                .SetFont(BoldFont)
                .SetBorder(borderWidth)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .Add(new Paragraph(columns[i]).SetMultipliedLeading(0.9f));

            table.AddHeaderCell(cell);
        }
        #endregion Table Headers

        return table;
    }

    public override void Render(Document container)
    {
        if (Tables?.Count > 0)
        {
            float progress = 0.3f;
            int currentRow = 0;

            var ci = new System.Globalization.CultureInfo("it-IT");

            var subTotals = IntermediateTableTotalLocation == IntermediateTotalLocation.TableBottom
                ? new List<IList<Cell>>()
                : null;

            if (!string.IsNullOrWhiteSpace(Title))
            {
                container.Add(
                    new Paragraph(Title)
                        .SetFont(BoldFont)
                        .SetFontSize(12f)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    );
            }

            Table table = CreateTable();

            container.Add(table);

            var totalOfTotals = Tables.Select(_ => _.Rows.Count).Sum();

            foreach (var item in Tables)
            {
                var isRenderTotal = IntermediateTableTotalLocation == IntermediateTotalLocation.TableBody || Tables.Count == 1;

                item.FillSectionByData(table, flush, ci, ref currentRow, isRenderTotal);

                if (!isRenderTotal)
                {
                    subTotals.Add(PdfTable<T>.GetTotalCells(item.TotalRow, Columns, ci));
                }
            }

            if (Tables.Count > 1)
            {
                if (subTotals?.Any() ?? false)
                {
                    foreach (var item in subTotals)
                    {
                        foreach (var cell in item)
                            table.AddCell(cell);
                    }
                    table.Flush();
                }
            }

            PdfTable<T>.AddTotalToTable(table, PdfTable<T>.GetTotalCells(MainTotalRow, Columns, ci));

            table.Complete();

            void flush()
            {
                table.Flush();

                var updatedProgress = progress + (currentRow * 0.35f) / totalOfTotals;

                if (updatedProgress < 1.0f)
                    Progress?.Report(updatedProgress);
            }
        }
        else
        {
            container.Add(new Paragraph("NO DATA TO DISPLAY").SetFont(ItalicFont).SetBold().SetFontSize(14f));
        }
    }
}