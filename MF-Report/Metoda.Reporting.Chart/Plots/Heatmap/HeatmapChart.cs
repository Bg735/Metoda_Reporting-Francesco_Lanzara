using Metoda.Reporting.Chart.Extensions;
using Metoda.Reporting.Chart.Plots.Base;
using Metoda.Reporting.Common.Enums;
using ScottPlot;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SD = System.Drawing;
using System.Linq;
using System.Drawing;
using System.IO;

namespace Metoda.Reporting.Chart.Plots.Heatmap;
public sealed class HeatmapChart : ChartBase<HeatmapOptions>
{
    private readonly Heatmap[] _intensities;

    private HeatmapChart(Heatmap[] intensities, string title = null, HeatmapOptions options = null)
        : base(ChartType.Heatmap, title, options)
    {
        _intensities = intensities;
    }

    protected override void FillPlot(Plot plt)
    {
        int colorBarSpace = 100;
        float borderAriaWidth = 152;
        float borderAriaHeight = 72;
        float fontSize12 = 12;
        float fontSize10 = 10;
        string valueFmt = Options.UsePercentageValue ? "{0:0.00%}" : "{0:0.00}";
        double transformationCoeff = 75;
        int iconEdge = 40;
        float iconXOffset = 0.23f;
        float iconYOffset = 0.07f;
        var iconSize = new SD.Size(iconEdge, iconEdge);

        var valueArray = _intensities.Select(_ => _.Value).ToArray();
        var values = valueArray.ConvertOneDimArrayToTwoDim(Options.ColCount);
        var labels = _intensities.Select(_ => _.Label).ToArray().ConvertOneDimArrayToTwoDim(Options.ColCount);
        var icons = _intensities.Select(_ => _.Icon).ToArray().ConvertOneDimArrayToTwoDim(Options.ColCount);

        int rows = values.GetLength(0);
        int cols = values.GetLength(1);

        plt.Width = borderAriaWidth + 75 * cols;
        plt.Height = borderAriaHeight + 75 * rows;

        var hmc = plt.AddHeatmap(values);

        var colors = GetColors(valueArray, hmc.Colormap).ConvertOneDimArrayToTwoDim(Options.ColCount);


        string fontFamilyName = "Arial";
        FontStyle fontStyle = FontStyle.Bold;

        using var font12 = new SD.Font(fontFamilyName, fontSize12, fontStyle, GraphicsUnit.Pixel);
        using var font10 = new SD.Font(fontFamilyName, fontSize10, fontStyle, GraphicsUnit.Pixel);

        using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
        {
            var fontHeight = GetStirngSize("X", font12, graphics).Height;
            double labelRowCoeff = 1 - fontHeight * 2 / transformationCoeff;
            double valueRowCoeff = 1 - fontHeight / transformationCoeff;

            for (int row = rows, textRowIdx = 0; row > 0; row--, textRowIdx++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var label = labels[textRowIdx, col];

                    if (string.IsNullOrEmpty(label)) continue;

                    // Logo                 
                    using var ms = new MemoryStream(icons[textRowIdx, col]);
                    using Image image = Image.FromStream(ms);
                    plt.AddImage(new Bitmap(image, iconSize), col + iconXOffset, row - iconYOffset);

                    var foreColor = colors[textRowIdx, col].GetForegroundColorForBackground();

                    SizeF txtSize = GetStirngSize(label, font12, graphics);
                    var txtX = col + (1 - txtSize.Width / transformationCoeff) / 2;
                    var txtY = row - labelRowCoeff;
                    var plotText = plt.AddText(label, txtX, txtY, fontSize12, foreColor);
                    plotText.Font.Bold = true;

                    // Value
                    var value = string.Format(valueFmt, values[textRowIdx, col]);
                    txtSize = GetStirngSize(value, font10, graphics);
                    txtX = col + (1 - txtSize.Width / transformationCoeff) / 2;
                    txtY = row - valueRowCoeff;
                    plotText = plt.AddText(value, txtX, txtY, fontSize10, foreColor);
                    plotText.Font.Bold = true;
                }
            }
        }

        plt.AddColorbar(hmc, colorBarSpace);
        plt.Margins(0, 0);
    }

    private static SizeF GetStirngSize(string text, SD.Font fnt, Graphics gfx)
    {
        return gfx.MeasureString(text, fnt);
    }

    public static HeatmapChart Create(
            List<Heatmap> intensities,
            string title = null,
            HeatmapOptions options = null
       )
    {
        if (!intensities?.Any() ?? true)
            throw new ArgumentException($"Parameter '{nameof(intensities)}' must not be null or empty.");

        return new HeatmapChart(intensities.ToArray(), title, options);
    }

    private static Color[] GetColors(double?[] intensities, Colormap colorMap)
    {
        double?[] intensitiesFlattened = intensities.Cast<double?>().ToArray();

        double Min = double.PositiveInfinity;
        double Max = double.NegativeInfinity;

        foreach (double? curr in intensities)
        {
            if (curr.HasValue && double.IsNaN(curr.Value))
                throw new ArgumentException("Heatmaps do not support intensities of double.NaN");

            if (curr.HasValue && curr.Value < Min)
                Min = curr.Value;

            if (curr.HasValue && curr.Value > Max)
                Max = curr.Value;
        }

        double?[] NormalizedIntensities = Normalize(intensitiesFlattened, Min, Max);

        int[] flatARGB = Colormap.GetRGBAs(NormalizedIntensities, colorMap, double.NegativeInfinity);


        Color[] colors = new Color[flatARGB.Length];
        for (int i = 0; i < flatARGB.Length; i++)
        {
            if (!intensities[i].HasValue)
            {
                colors[i] = Color.FromArgb(255, 255, 255, 255);
            }
            else
            {
                colors[i] = Color.FromArgb(flatARGB[i]);
            }

            Debug.WriteLine($"i: {i}|Color: {colors[i]} | Intensity: {intensities[i]}");
        }
        return colors;
    }

    private static double?[] Normalize(double?[] input, double? min = null, double? max = null)
    {
        double? NormalizePreserveNull(double? i)
        {
            if (i.HasValue)
            {
                return (i.Value - min.Value) / (max.Value - min.Value);
            }
            return null;
        }

        double?[] normalized = input.AsParallel().AsOrdered().Select(_ => NormalizePreserveNull(_)).ToArray();

        return normalized;
    }
}
