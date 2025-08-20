using System;

namespace Metoda.Reporting.Chart.Plots.Pie;
public class Pie
{
    public string Label { get; private set; }
    public double Value { get; private set; }

    public Pie(double value, string label)
    {
        if (string.IsNullOrWhiteSpace(label))
            throw new ArgumentException($"Parameter '{nameof(label)}' must not be null or empty.");

        Label = label;
        Value = value;
    }
}
