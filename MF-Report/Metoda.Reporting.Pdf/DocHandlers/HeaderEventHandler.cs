using iText.IO.Image;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using Metoda.Reporting.Common.Res;
using System;
using System.Diagnostics;

namespace Metoda.Reporting.Pdf.DocHandlers;

public class HeaderEventHandler : IEventHandler
{
    private readonly float _tableHeight;

    private readonly Table _1stPageTable;
    private readonly Table _otherPagesTable;
    private readonly Document _doc;
    private readonly PdfFont[] _fonts;
    private readonly float _docTopMargin;

    public HeaderEventHandler(Document doc, PdfFont[] fonts, string title)
    {
        _doc = doc;
        _fonts = fonts;
        _1stPageTable = GetHeaderTable(title);
        _otherPagesTable = GetHeaderTable();
        _docTopMargin = _doc.GetTopMargin();

        Debug.WriteLine($"Header Height: {_1stPageTable.GetHeight()}");

        TableRenderer renderer = (TableRenderer)_1stPageTable.CreateRendererSubTree();
        renderer.SetParent(new DocumentRenderer(doc));

        renderer = (TableRenderer)_otherPagesTable.CreateRendererSubTree();
        renderer.SetParent(new DocumentRenderer(doc));

        LayoutResult result = renderer.Layout(new LayoutContext(new LayoutArea(0, PageSize.A4)));
        _tableHeight = result.GetOccupiedArea().GetBBox().GetHeight();
    }

    public void HandleEvent(Event evt)
    {
        PdfDocumentEvent docEvent = (PdfDocumentEvent)evt;
        PdfDocument pdfDoc = docEvent.GetDocument();

        PdfPage page = docEvent.GetPage();

        int pageNum = docEvent.GetDocument().GetPageNumber(page);
        var table = pageNum == 1 ? _1stPageTable : _otherPagesTable;
        PdfCanvas canvas = new(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
        PageSize pageSize = pdfDoc.GetDefaultPageSize();
        var margin = pageNum == 1 && table.GetHeight() != null ? table.GetHeight().GetValue() : _docTopMargin;

        _doc.SetTopMargin(margin + 36f);

        float coordX = pageSize.GetX() + _doc.GetLeftMargin();
        float coordY = pageSize.GetTop() - _doc.GetTopMargin();
        float width = pageSize.GetWidth() - _doc.GetRightMargin() - _doc.GetLeftMargin();
        float height = GetTableHeight();
        Rectangle rect = new(coordX, coordY, width, height);

        new Canvas(canvas, rect).Add(table).Close();
    }

    public float GetTableHeight()
    {
        return _tableHeight;
    }

    private Table GetHeaderTable(string title = null)
    {
        Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1.5f, 6f, 2.5f }))
            .SetWidth(UnitValue.CreatePercentValue(100f))
            .SetBorder(Border.NO_BORDER)
            .SetFontSize(11f)
            .SetFont(_fonts[1])
            .SetBorderBottom(new SolidBorder(0.5f));

        Cell cell = new Cell()
            .SetBorder(Border.NO_BORDER)
            .SetHorizontalAlignment(HorizontalAlignment.LEFT)
            .SetVerticalAlignment(VerticalAlignment.BOTTOM);

        Image image = new Image(ImageDataFactory.Create(Resource.logo_metoda))
            .SetHeight(24f)
            .SetPaddingLeft(10f);

        cell.Add(image);
        table.AddCell(cell);

        if (title != null)
        {
            var cellValue = new Paragraph(title).SetMultipliedLeading(1.2f);
            Cell titleCell = new Cell()
            .SetBorder(Border.NO_BORDER)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetVerticalAlignment(VerticalAlignment.BOTTOM)
            .Add(cellValue);

            table.AddCell(titleCell);

            cell = new Cell()
            .SetBorder(Border.NO_BORDER)
            .SetTextAlignment(TextAlignment.RIGHT)
            .SetFont(_fonts[2])
            .SetVerticalAlignment(VerticalAlignment.BOTTOM)
            .Add(new Paragraph($"Data Stampa: {DateTime.Now:dd/MM/yyyy}"));

            table.AddCell(cell);
        }
        table.SetHeight(36f);

        return table;
    }
}