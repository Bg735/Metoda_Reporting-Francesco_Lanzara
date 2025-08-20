using System.Linq;
using System;

namespace Metoda.Reporting.Chart.Plots.Bar;

public class Bar
{
    public string Label { get; set; }
    public double [] Values { get; set; }

    public Bar(double[] values, string label)
    {
        if (string.IsNullOrWhiteSpace(label))
            throw new ArgumentException($"Parameter '{nameof(label)}' must not be null or empty.");

        if (!values?.Any() ?? true)
            throw new ArgumentException($"Parameter '{nameof(values)}' must not be null or empty.");

        Label = label;
        Values = values;
    }
}
