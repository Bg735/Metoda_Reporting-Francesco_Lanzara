using iText.Kernel.Font;
using iText.Layout;

namespace Metoda.Reporting.Pdf.ReportElements;

public class PdfDocumentNestedSectionItem : PdfNestedSectionItem<Document>
{
    public PdfDocumentNestedSectionItem(float fontSize, float leftMargin, PdfFont regular, PdfFont bold)
        : base(fontSize, leftMargin, regular, bold) 
    {
    }
}
