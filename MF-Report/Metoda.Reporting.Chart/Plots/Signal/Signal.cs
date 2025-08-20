using System.Linq;
using System;

namespace Metoda.Reporting.Chart.Plots.Signal;
public class Signal
{
    public string Label { get; private set; }
    public double[] YValues { get; private set; }

    public Signal(double[] yValues, string label)
    {
        if (string.IsNullOrWhiteSpace(label))
            throw new ArgumentException($"Parameter '{nameof(label)}' must not be null or empty.");

        if (!yValues?.Any() ?? true)
            throw new ArgumentException($"Parameter '{nameof(yValues)}' must not be null or empty.");

        Label = label;
        YValues = yValues;
    }
}