using iText.Kernel.Font;
using iText.Layout.Element;

namespace Metoda.Reporting.Pdf.ReportElements;

public class PdfNestedTableItemSectionItem : PdfNestedSectionItem<Table>
{
    public PdfNestedTableItemSectionItem(float fontSize, float leftMargin, PdfFont regular, PdfFont bold)
        : base(fontSize, leftMargin, regular, bold) { }
}