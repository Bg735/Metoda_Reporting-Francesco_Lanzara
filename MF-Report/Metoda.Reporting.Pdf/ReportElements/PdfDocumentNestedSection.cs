using iText.Kernel.Font;
using iText.Layout;

namespace Metoda.Reporting.Pdf.ReportElements;

public class PdfDocumentNestedSection : PdfNestedSectionBase<Document, PdfDocumentNestedSectionItem>
{
    public PdfDocumentNestedSection(float fontSize, float leftMargin, PdfFont bold) 
        : base(fontSize, leftMargin, bold) 
    { 
    }
}