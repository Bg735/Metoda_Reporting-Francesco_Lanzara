using System;
using System.Linq;

namespace Metoda.Reporting.Chart.Plots.Scatter;

public class ScatterAxis
{
    public string Label { get; private set; }
    public double[] Values { get; private set; }

    public ScatterAxis(double[] values, string label)
    {
        if (string.IsNullOrWhiteSpace(label))
            throw new ArgumentException($"Parameter '{nameof(label)}' must not be null or empty.");

        if (!values?.Any() ?? true)
            throw new ArgumentException($"Parameter '{nameof(values)}' must not be null or empty.");

        Label = label;
        Values = values;
    }
}
