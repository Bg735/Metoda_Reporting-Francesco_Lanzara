using Metoda.Reporting.Chart.Plots.Base;
using ScottPlot;

namespace Metoda.Reporting.Chart.Plots.BarSeries;

public class BarSeriesOptions : BarOptionsBase, ILegendedOptions
{
    public bool ShowLegend { get; set; } = true;
    public Alignment LegendAlignment { get; set; } = Alignment.LowerRight;
}