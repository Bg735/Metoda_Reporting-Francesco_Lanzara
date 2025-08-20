using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using Metoda.Reporting.Common.Elements.ReportElements;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Pdf.ReportElements;

public abstract class PdfColumnSectionItem<TContainer>
    : ColumnSectionItemBase<TContainer, Table>
where TContainer : class
{
    protected readonly float _fontSize;
    protected readonly float _topMargin;
    protected readonly PdfFont _font;
    protected readonly float[] _columnWidths;
    public PdfColumnSectionItem(float fontSize, float topMargin, PdfFont font, float[] columnWidths)
    {
        _fontSize = fontSize;
        _topMargin = topMargin;
        _font = font;
        _columnWidths = columnWidths;
    }

    //public override void Render(TContainer container)
    //{
    //    Render(container);
    //}

    //public virtual void RenderContent(Table container)
    //{
    //    Render(container);
    //}

    //private void Render<T>(T container)
    //{
        
    //    if (Content?.Any() ?? false)
    //    {
    //        if (container is Document)
    //        {
    //            foreach (var item in Content)
    //            {
    //                (container as Document).Add(GetParagraph(item));
    //            }
    //        }
    //        else if (container is Table)
    //        {
    //            foreach (var item in Content)
    //            {
    //                (container as Table).AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(GetParagraph(item)));
    //            }
    //        }
    //    }
    //}

    //private Paragraph GetParagraph( string row)
    //{
    //    Paragraph p = new();
    //    foreach (var col in row)
    //    {
    //        p.Add(new Text($"{col}: ").SetFont(_font).SetFontSize(_fontSize));
    //    }
    //    p.SetMarginLeft(_toptMargin + 10f);
    //    return p;
    //}
}