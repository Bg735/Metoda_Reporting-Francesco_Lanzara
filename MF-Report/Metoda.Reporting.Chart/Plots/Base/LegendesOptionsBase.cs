using ScottPlot;

namespace Metoda.Reporting.Chart.Plots.Base;

public abstract class LegendedOptionsBase : OptionsBase, ILegendedOptions
{
    public bool ShowLegend { get; set; } = true;
    public Alignment LegendAlignment { get; set; } = Alignment.LowerRight;
}
