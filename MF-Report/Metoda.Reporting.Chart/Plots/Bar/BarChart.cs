using Metoda.Reporting.Chart.Plots.Base;
using Metoda.Reporting.Common.Enums;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Chart.Plots.Bar;
public sealed class BarChart : BarChartBase<BarOptions>
{
    private readonly Bar _bar;

    private BarChart(Bar bar, string title = null, BarOptions options = null) : base(ChartType.Bar, title, options)
    {
        _bar = bar;
    }

    protected override IEnumerable<double> UpdatePlot(Plot plt)
    {
        var bar = plt.AddBar(_bar.Values);
        
        bar.ShowValuesAboveBars = Options.ShowValuesAboveBars;

        plt.YLabel(_bar.Label);

        return (_bar.Values);    
    }

    public static BarChart Create(Bar bar, string title = null, BarOptions options = null)
    {
        if (!bar?.Values?.Any() ?? true)
            throw new ArgumentException($"Parameter '{nameof(bar)}' must not be null and its {nameof(bar.Values)} must not be empty");

        return new BarChart(bar, title, options);
    }
}
