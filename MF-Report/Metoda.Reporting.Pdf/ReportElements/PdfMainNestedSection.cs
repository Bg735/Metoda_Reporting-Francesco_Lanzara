using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using System.Linq;

namespace Metoda.Reporting.Pdf.ReportElements;

public class PdfMainNestedSection : PdfNestedSectionItem<Document>
{
    public PdfMainNestedSection(float fontSize, float leftMargin, PdfFont regular, PdfFont bold)
        : base(fontSize, leftMargin, regular, bold)
    {
    }

    public override void Render(Document container)
    {
        if (!string.IsNullOrWhiteSpace(Title) || (Content?.Any() ?? false))
        {
            Table tableContainer = new Table(1)
                .UseAllAvailableWidth()
                .SetMarginTop(1.0f)
                .SetMarginBottom(1.0f)
                .SetBorder(new SolidBorder(0.5f));

            if (!string.IsNullOrWhiteSpace(Title))
            {
                Paragraph p = new();
                p.Add(new Text($"{Title}: ").SetFont(_bold).SetFontSize(_fontSize));

                if (!string.IsNullOrWhiteSpace(TitleEnd))
                {
                    p.Add(new Text($"{TitleEnd}").SetFont(_regular).SetFontSize(_fontSize));
                }

                p.SetMarginLeft(_leftMargin);

                tableContainer.AddHeaderCell(new Cell().SetBorder(Border.NO_BORDER).Add(p));
            }

            if (Content?.Any() ?? false)
            {
                float subFontSize = _fontSize - 1;

                foreach (var item in Content)
                {
                    Paragraph p = new();
                    p.Add(new Text($"{item.Key}: ").SetFont(_bold).SetFontSize(subFontSize));
                    p.Add(new Text($"{item.Value}").SetFont(_regular).SetFontSize(subFontSize));
                    p.SetMarginLeft(_leftMargin + 10f);

                    tableContainer.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(p));
                }
            }

            container.Add(tableContainer);
        }
    }
}