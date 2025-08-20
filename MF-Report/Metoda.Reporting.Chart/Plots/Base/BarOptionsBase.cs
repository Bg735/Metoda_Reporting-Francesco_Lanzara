namespace Metoda.Reporting.Chart.Plots.Base;

public abstract class BarOptionsBase : OptionsBase
{
    public double? XAxisLimitMin { get; set; } = null;
    public double? XAxisLimitMax { get; set; } = null;
    public double? YAxisLimitMin { get; set; } = 0;
    public double? YAxisLimitMax { get; set; } = null;
}
