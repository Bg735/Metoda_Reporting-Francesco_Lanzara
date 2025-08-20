using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using Metoda.Reporting.Common.Elements.ReportElements;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Pdf.ReportElements;

public abstract class PdfColumnSectionBase<TContent, TListItem>
        : SectionContainerBase<IList<TListItem>, TContent, Table>
    where TContent : class
    where TListItem : PdfColumnSectionItem<TContent>
{
    protected readonly float _fontSize;
    protected readonly float _leftMargin;
    protected readonly PdfFont _bold;

    public PdfColumnSectionBase(float fontSize, float leftMargin, PdfFont bold)
    {
        _fontSize = fontSize;
        _leftMargin = leftMargin;
        _bold = bold;
    }

    //public override void Render(TContent container)
    //{
    //    Table tableContainer = new Table(1)
    //    .UseAllAvailableWidth()
    //    .SetMarginTop(1.0f)
    //    .SetMarginBottom(1.0f)
    //    .SetBorder(new SolidBorder(0.5f));

    //    if (Content?.Any() ?? false)
    //    {
    //        foreach (var item in Content)
    //        {
    //            item.RenderContent(tableContainer);
    //        }
    //    }

    //    if (container is Document)
    //    {
    //        (container as Document).Add(tableContainer);
    //    }
    //    else if (container is Table)
    //    {
    //        (container as Table).AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(tableContainer));
    //    }
    //    else
    //    {
    //        throw new System.Exception("Unsupported continer type");
    //    }
    //}
}