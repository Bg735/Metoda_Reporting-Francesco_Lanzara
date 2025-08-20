using iText.IO.Image;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.ReportELements;

namespace Metoda.Reporting.Pdf.ReportElements;

public class PdfReportChart : ReportChartBase<Document>
{
    public float _widthInPixels;

    public PdfReportChart(IChart chart, float widthInPixels = 0f) : base(chart)
    {
        _widthInPixels = widthInPixels;
    }

    public override void Render(Document container)
    {
        byte[] imageData = _chart.GetImageAsByteArray();

        Image image = new Image(ImageDataFactory.Create(imageData));


        float cellWidth = container.GetPdfDocument().GetDefaultPageSize().GetWidth()
                            - container.GetLeftMargin()
                            - container.GetRightMargin();
        image.SetWidth(cellWidth);

        if (_widthInPixels > 0f)
        {
            // Create a UnitValue object from pixels
            UnitValue unitValue = UnitValue.CreatePointValue(_widthInPixels / 2.835f);
            image.SetWidth(unitValue);
        }
        Table table = new Table(1)
                               .UseAllAvailableWidth()
                               .SetBorder(Border.NO_BORDER);

        Cell cell = new Cell()
            .SetBorder(Border.NO_BORDER)
            .Add(image);

        table.AddCell(cell);

        container.Add(table);
    }
}