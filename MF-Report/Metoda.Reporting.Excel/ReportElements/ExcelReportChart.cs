using Metoda.Reporting.Common.Elements.ReportELements;
using NPOI.SS.UserModel;
using MetodaContracts = Metoda.Reporting.Common.Elements.Contracts;

namespace Metoda.Reporting.Excel.ReportElements;

public class ExcelReportChart : ReportChartBase<ISheet>
{
    public float _widthInPixels;

    public ExcelReportChart(MetodaContracts.IChart chart, float widthInPixels = 0f) : base(chart)
    {
        _widthInPixels = widthInPixels;
    }

    public override void Render(ISheet container, int cols)
    {
        byte[] imageData = _chart.GetImageAsByteArray();
        int pictureIndex = container.Workbook.AddPicture(imageData, PictureType.JPEG);
        //ICreationHelper helper = container.Workbook.GetCreationHelper();
        IDrawing drawing = container.CreateDrawingPatriarch();
        //IClientAnchor anchor = helper.CreateClientAnchor();
        int lastRowNum = container.LastRowNum + 1;
        ICell cell = container.CreateRow(lastRowNum).CreateCell(0);

        IClientAnchor anchor = drawing.CreateAnchor(0, 0, 0, 0, cell.ColumnIndex, cell.RowIndex, cell.ColumnIndex + 1, cell.RowIndex + 1);

        // Set the size of the anchor to match the desired image size (600x400 pixels)
        anchor.AnchorType = AnchorType.MoveDontResize; // Move and size with cells
        anchor.Dx1 = 0;
        anchor.Dx2 = 0;
        anchor.Dy1 = 0;
        anchor.Dy2 = 0;
        anchor.Col1 = cell.ColumnIndex;
        anchor.Col2 = cell.ColumnIndex + 6; // E column has a width of 7 units, so we use 6 units for a 600px wide image
        anchor.Row1 = cell.RowIndex;
        anchor.Row2 = cell.RowIndex + 6; // 400px tall image (6 units)


        //anchor.Col1 = 0;//0 index based column
        //anchor.Row1 = container.LastRowNum + 1;// last index based row
       
        IPicture picture = drawing.CreatePicture(anchor, pictureIndex);
        picture.Resize();
    }
}