using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Metoda.Reporting.Common.Elements.ReportElements;

namespace Metoda.Reporting.Pdf.ReportElements
{

    public class PdfSimpleText : ReportElement<Document>
    {
        protected readonly float _fontSize;
        protected readonly float _topMargin;
        protected readonly float _leftMargin;
        protected readonly PdfFont _font;
        protected readonly string _text;

        public PdfSimpleText(string text, float fontSize, PdfFont font, float topMargin = 12f, float leftMargin = 0f) { 
            _fontSize = fontSize;
            _font = font;
            _topMargin = topMargin;
            _leftMargin = leftMargin;
            _text = text;
        }


        public override void Render(Document container)
        {

            Table tableContainer = new Table(1)
               .UseAllAvailableWidth()
               .SetBorder(Border.NO_BORDER);

            Paragraph p = new();            
            p.Add(new Text(_text).SetFont(_font).SetFontSize(_fontSize));
            p.SetTextAlignment(TextAlignment.CENTER);
            p.SetMarginTop(_topMargin);
            p.SetMarginLeft(_leftMargin);           
            container.Add(p);

        }

    }
}
