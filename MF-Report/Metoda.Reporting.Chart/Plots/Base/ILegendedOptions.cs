using ScottPlot;

namespace Metoda.Reporting.Chart.Plots.Base;
public interface ILegendedOptions
{
    Alignment LegendAlignment { get; set; }
    bool ShowLegend { get; set; }
}