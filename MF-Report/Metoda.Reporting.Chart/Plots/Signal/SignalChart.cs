using Metoda.Reporting.Chart.Plots.Base;
using Metoda.Reporting.Common.Enums;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Chart.Plots.Signal;
public sealed class SignalChart : ChartBase<SignalOptions>
{
    private readonly List<Signal> _yAxisSignalList;

    private SignalChart(List<Signal> yAxisSignalList, string title = null, SignalOptions options = null) : base(ChartType.Signal, title, options)
    {
        _yAxisSignalList = yAxisSignalList;
    }

    protected override void FillPlot(Plot plt)
    {
        foreach (Signal signal in _yAxisSignalList)
        {
            plt.AddSignal(signal.YValues, label: signal.Label);
        }
    }

    public static SignalChart Create(
           List<Signal> yAxisSignalList,
           string title = null,
           SignalOptions options = null
       )
    {
        if (!yAxisSignalList?.Any() ?? true)
            throw new ArgumentException($"Parameter '{nameof(yAxisSignalList)}' must not be null or empty.");

        return new SignalChart(yAxisSignalList, title, options);
    }
}
