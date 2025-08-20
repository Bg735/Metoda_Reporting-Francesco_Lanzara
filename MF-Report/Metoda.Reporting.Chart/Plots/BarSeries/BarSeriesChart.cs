using Metoda.Reporting.Common.Enums;
using ScottPlot;
using SPBar = ScottPlot.Plottable.Bar;
using System;
using System.Collections.Generic;
using System.Linq;
using Metoda.Reporting.Chart.Plots.Base;

namespace Metoda.Reporting.Chart.Plots.BarSeries;

public sealed class BarSeriesChart : BarChartBase<BarSeriesOptions>
{
    private readonly List<BarSerie> _series;

    private BarSeriesChart(List<BarSerie> series, string title = null, BarSeriesOptions options = null) : base(ChartType.BarSeries, title, options)
    {
        _series = series;
    }

    protected override IEnumerable<double> UpdatePlot(Plot plt)
    {
        List<SPBar> bars = new();

        for (int i = 0; i < _series.Count; i++)
        {
            var serie = _series[i];

            bars.Add(new()
            {
                Value = serie.Value,
                Position = serie.Position ?? i,
                FillColor = Palette.Category10.GetColor(i),
                Label = serie.Label ?? serie.Value.ToString(),
                LineWidth = serie.LineWidth                
            });
        };

        plt.AddBarSeries(bars);

        return _series.Select(_ => _.Value);

    }

    public static BarSeriesChart Create(List<BarSerie> series, string title = null, BarSeriesOptions options = null)
    {
        if (!series?.Any() ?? true)
            throw new ArgumentException($"Parameter '{nameof(series)}' must not be null or empty");

        return new BarSeriesChart(series, title, options);
    }
}