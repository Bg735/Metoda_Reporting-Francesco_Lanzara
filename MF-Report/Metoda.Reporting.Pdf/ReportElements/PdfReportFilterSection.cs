using iText.IO.Font;
using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Metoda.Reporting.Common.Elements.ReportElements;
using Metoda.Reporting.Common.Res;

namespace Metoda.Reporting.Pdf.ReportElements;

public class PdfReportFilterSection : ReportFilterSectionBase<Document>
{
    protected readonly PdfFont _boldFont = PdfFontFactory.CreateFont(
        Resource.CALIBRIB,
        PdfEncodings.WINANSI,
        PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

    public PdfReportFilterSection() : base()
    {
    }

    public PdfReportFilterSection(string label, string content)
        : base(label, content)
    {
    }

    public override void Render(Document container)
    {
        Table table = new Table(1)
                               .UseAllAvailableWidth()
                               .SetBorder(new SolidBorder(0.5f))
                               .SetFontSize(FontSize)
                               .SetFont(_boldFont)
                               .SetMarginTop(1.0f)
                               .SetMarginBottom(1.0f);

        table.AddCell(new Cell()
                            .SetBorder(Border.NO_BORDER)
                            .SetTextAlignment(TextAlignment.LEFT)
                            .Add(new Paragraph()
                                        .SetMarginLeft(10f)
                                        .Add(new Text($"{Label?.Trim()}: "))
                                        .Add(new Text(Content?.Trim()).SetFontSize(FontSize - 2))));

        container.Add(table);
    }
}
