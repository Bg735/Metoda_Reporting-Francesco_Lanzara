using System.Linq;
using System;

namespace Metoda.Reporting.Chart.Plots.Heatmap;
public class Heatmap
{
    public string Label { get; private set; }
    public double? Value { get; private set; }
    public byte[] Icon { get; private set; }

    public Heatmap(double? value, string label, byte[] icon)
    {
        if (string.IsNullOrWhiteSpace(label))
            throw new ArgumentException($"Parameter '{nameof(label)}' must not be null or empty.");
        
        if (!value.HasValue)
            throw new ArgumentException($"Parameter '{nameof(value)}' must not be null.");

        if (!icon?.Any() ?? true)
            throw new ArgumentException($"Parameter '{nameof(icon)}' must not be null or empty.");

        Label = label;
        Value = value;
        Icon = icon;
    }
}
