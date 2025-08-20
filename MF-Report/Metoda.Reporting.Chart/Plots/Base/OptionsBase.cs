using System.Drawing;

namespace Metoda.Reporting.Chart.Plots.Base;

public abstract class OptionsBase
{
    public Size PlotDimensions { get; set; } = new Size(800, 600);
}
