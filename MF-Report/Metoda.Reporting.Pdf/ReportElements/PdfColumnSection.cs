using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Linq;

namespace Metoda.Reporting.Pdf.ReportElements;

public class PdfColumnSection : PdfColumnSectionItem<Document>
{
    public PdfColumnSection(float fontSize, PdfFont font, float[] columnSizes, float topMargin = 12f)
        : base(fontSize, topMargin,font, columnSizes)
    {
    }

    public override void Render(Document container)
    {
        if ((Content?.Any() ?? false))
        {
            Table tableContainer = new Table(UnitValue.CreatePercentArray(_columnWidths))
                .UseAllAvailableWidth()
                .SetMarginTop(_topMargin)
                .SetMarginBottom(1.0f)
                .SetBorder(Border.NO_BORDER)
                .SetFont(_font)
                .SetFontSize(_fontSize);


            foreach (var cell in Content)
            {

                Paragraph p = new();
                p.Add(new Text(cell));
             
                tableContainer.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(p));
            }
            container.Add(tableContainer);
        }
    }
}