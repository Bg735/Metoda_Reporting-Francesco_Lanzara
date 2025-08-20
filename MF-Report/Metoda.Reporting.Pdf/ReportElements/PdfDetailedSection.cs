using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using Metoda.Reporting.Common.Elements.ReportElements;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Pdf.ReportElements;

public class PdfDetailedSection : SectionContainerBase<IEnumerable<PdfDetailedSectionItem>, Document, Document>
{
    private readonly float _fontSize;
    private readonly PdfFont _bold;

    public PdfDetailedSection(float fontSize, PdfFont bold)
    {
        _fontSize = fontSize;
        _bold = bold;
    }

    public override void Render(Document container)
    {
        if (!string.IsNullOrWhiteSpace(Title))
        {
            var halfThickBorder = new SolidBorder(0.5f);
            var fullThickBorder = new SolidBorder(1.0f);

            Table tableContainer = new Table(1)
                .UseAllAvailableWidth()
                .SetBorder(halfThickBorder)
                .SetMarginTop(1.0f)
                .SetMarginBottom(1.0f);

            Paragraph p = new Paragraph($"{Title}").SetFont(_bold).SetFontSize(_fontSize);

            tableContainer.AddHeaderCell(
                new Cell().SetBorder(Border.NO_BORDER)
                    .SetBorderBottom(fullThickBorder)
                    .Add(p));

            container.Add(tableContainer);
        }

        if (Content?.Any() ?? false)
        {
            foreach (var item in Content)
            {
                item.Render(container);
            }
        }
    }
}

