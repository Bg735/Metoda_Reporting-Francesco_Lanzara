using iText.IO.Font;
using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Metoda.Reporting.Common.Elements.ReportElements;
using Metoda.Reporting.Common.Res;
using System;

namespace Metoda.Reporting.Pdf.ReportElements;

public class PdfReportCompanyLine : ReportCompanyLineBase<Document>
{
    protected readonly PdfFont _boldFont = PdfFontFactory.CreateFont(
        Resource.CALIBRIB,
        PdfEncodings.WINANSI,
        PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

    public PdfReportCompanyLine(string companyName, DateTime referenceDate) 
        : base(companyName, referenceDate) 
    { 
    }

    public override void Render(Document container)
    {
        Table table = new Table(2)
                               .UseAllAvailableWidth()
                               .SetBorder(Border.NO_BORDER)
                               .SetFontSize(FontSize)
                               .SetFont(_boldFont);

        Cell cell = new Cell()
            .SetBorder(Border.NO_BORDER)
            .SetTextAlignment(TextAlignment.LEFT)
            .Add(new Paragraph(_companyName));

        table.AddCell(cell);

        cell = new Cell()
            .SetBorder(Border.NO_BORDER)
            .SetTextAlignment(TextAlignment.RIGHT)
            .Add(new Paragraph($"Data Riferimento: {_referenceDate:dd/MM/yyyy}"));

        table.AddCell(cell);
        container.Add(table);
    }
}