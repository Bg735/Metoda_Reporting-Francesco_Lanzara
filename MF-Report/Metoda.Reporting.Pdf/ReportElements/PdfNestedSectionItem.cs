using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using Metoda.Reporting.Common.Elements.ReportElements;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Pdf.ReportElements;

public abstract class PdfNestedSectionItem<TContainer>
    : NestedSectionItemBase<TContainer, Table>
where TContainer : class
{
    protected readonly float _fontSize;
    protected readonly float _leftMargin;
    protected readonly PdfFont _regular;
    protected readonly PdfFont _bold;

    public PdfNestedSectionItem(float fontSize, float leftMargin, PdfFont regular, PdfFont bold)
    {
        _fontSize = fontSize;
        _leftMargin = leftMargin;
        _regular = regular;
        _bold = bold;
    }

    public override void Render(TContainer container)
    {
        Render(container);
    }

    public virtual void RenderContent(Table container)
    {
        Render(container);
    }

    private void Render<T>(T container)
    {
        if (!string.IsNullOrWhiteSpace(Title))
        {
            Paragraph p = new();
            p.Add(new Text($"{Title}: ").SetFont(_bold).SetFontSize(_fontSize));

            if (!string.IsNullOrWhiteSpace(TitleEnd))
            {
                p.Add(new Text($"{TitleEnd}").SetFont(_regular).SetFontSize(_fontSize));
            }

            p.SetMarginLeft(_leftMargin);

            if (container is Document)
            {
                (container as Document).Add(p);
            }
            else if (container is Table)
            {
                (container as Table).AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(p));
            }
            else
            {
                throw new System.Exception("Unsupported continer type");
            }
        }

        if (Content?.Any() ?? false)
        {
            float subFontSize = _fontSize - 1;

            if (container is Document)
            {
                foreach (var item in Content)
                {
                    (container as Document).Add(GetParagraph(subFontSize, item));
                }
            }
            else if (container is Table)
            {
                foreach (var item in Content)
                {
                    (container as Table).AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(GetParagraph(subFontSize, item)));
                }
            }
        }
    }

    private Paragraph GetParagraph(float subFontSize, KeyValuePair<string, string> item)
    {
        Paragraph p = new();
        p.Add(new Text($"{item.Key}: ").SetFont(_bold).SetFontSize(subFontSize));
        p.Add(new Text($"{item.Value}").SetFont(_regular).SetFontSize(subFontSize));
        p.SetMarginLeft(_leftMargin + 10f);
        return p;
    }
}