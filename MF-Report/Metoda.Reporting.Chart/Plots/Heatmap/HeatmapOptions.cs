using Metoda.Reporting.Chart.Plots.Base;

namespace Metoda.Reporting.Chart.Plots.Heatmap;
public class HeatmapOptions : OptionsBase
{
    public int ColCount { get; set; } = 10;
    public bool UsePercentageValue { get; set; } = true;
}