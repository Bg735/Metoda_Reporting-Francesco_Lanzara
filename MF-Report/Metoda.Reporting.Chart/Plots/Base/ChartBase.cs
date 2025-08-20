using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Enums;
using ScottPlot;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Metoda.Reporting.Chart.Plots.Base
{
    public abstract class ChartBase<TOptions> : IChart
        where TOptions : OptionsBase, new()
    {
        private readonly ChartType _chartType;
        protected string _title;

        public ChartType ChartType => _chartType;

        public TOptions Options { get; protected set; }

        protected ChartBase(ChartType chartType, string title, TOptions options)
        {
            _chartType = chartType;
            _title = title ?? string.Empty;
            Options = options ?? new TOptions();
        }

        public virtual byte[] GetImageAsByteArray()
        {
            Plot plt = new Plot(Options.PlotDimensions.Width, Options.PlotDimensions.Height);
            plt.Title(_title);

            FillPlot(plt);

            if (Options is ILegendedOptions op)
            if (op.ShowLegend)
            {
                plt.Legend(location: op.LegendAlignment);
            }

            Bitmap bmp = plt.Render();
            bmp.SetResolution(1200, 1200);
            // Save the bitmap as a byte array
            byte[] imageBytes;
            using (var stream = new MemoryStream())
            {
                bmp.Save(stream, ImageFormat.Png);
                imageBytes = stream.ToArray();
            }

            return imageBytes;
        }

        protected abstract void FillPlot(Plot plt);

        public static double GetPropertyValueConvertedToDouble(object propertyValue)
        {
            if (propertyValue != null)
            {
                if (propertyValue is double doubleValue)
                {
                    return doubleValue;
                }
                else if (double.TryParse(propertyValue.ToString(), out double parsedValue))
                {
                    return parsedValue;
                }
            }

            return 0.0;
        }
    }
}
