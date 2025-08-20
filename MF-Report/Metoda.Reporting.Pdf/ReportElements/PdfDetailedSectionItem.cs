using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Metoda.Reporting.Common.Elements.ReportElements;
using System.Linq;

namespace Metoda.Reporting.Pdf.ReportElements;

public class PdfDetailedSectionItem : DetailedSectionItemBase<Document, Table>
{
    private readonly float _fontSize;
    private readonly float _leftMargin;
    private readonly PdfFont _regular;
    private readonly PdfFont _bold;

    public PdfDetailedSectionItem(float fontSize, float leftMargin, PdfFont regular, PdfFont bold)
    {
        _fontSize = fontSize;
        _leftMargin = leftMargin;
        _regular = regular;
        _bold = bold;
    }

    public override void Render(Document container)
    {
        if (!string.IsNullOrWhiteSpace(Title))
        {
            Table tableContainer = new Table(1)
                .UseAllAvailableWidth()
                .SetMarginTop(1.0f)
                .SetMarginBottom(1.0f)
                .SetBorder(new SolidBorder(0.5f));

            Paragraph p = new();
            p.Add(new Text($"{Title}").SetFont(_bold).SetFontSize(_fontSize));

            if (!string.IsNullOrWhiteSpace(TitleEnd))
            {
                p.Add(new Text($" {TitleEnd}").SetFont(_regular).SetFontSize(_fontSize));
            }

            p.SetMarginLeft(_leftMargin);

            tableContainer.AddHeaderCell(new Cell().SetBorder(Border.NO_BORDER).Add(p));

            Table table = new(UnitValue.CreatePercentArray(new float[] { 0.3f, 0.7f }));
            table.SetWidth(UnitValue.CreatePercentValue(100f))
                .SetFont(_regular)
                .SetFontSize(_fontSize)
                .SetMarginLeft(_leftMargin)
                .SetBorder(Border.NO_BORDER);

            table.AddHeaderCell(new Cell().SetFont(_bold).SetBorder(Border.NO_BORDER).Add(new Paragraph(KeyHeader)))
                .AddHeaderCell(new Cell().SetFont(_bold).SetBorder(Border.NO_BORDER).Add(new Paragraph(ValueHeader)));

            if (Content?.Any() ?? false)
            {
                foreach (var kvp in Content)
                {
                    table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(kvp.Key)))
                        .AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph(kvp.Value)));
                }
            }

            tableContainer.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(table));
            container.Add(tableContainer);
        }
    }
}
