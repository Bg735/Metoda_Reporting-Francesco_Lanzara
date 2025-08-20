using Metoda.Reporting.Chart.Plots.Base;
using System.ComponentModel.DataAnnotations;

namespace Metoda.Reporting.Chart.Plots.Pie;

public class PieOptions : LegendedOptionsBase
{
    public bool Explode { get; set; } = true;

    /// <summary>
    /// Allowed value range from 0.0 to 1.0
    /// </summary>
    public double DonutSize { get; set; } = 0.0;
    public bool ShowValues { get; set; } = false;
    public bool ShowPercentages { get; set; } = false;
    public double SizeScale { get; set; } = 1.0;
}