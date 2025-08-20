using Metoda.Reporting.Chart.Plots.Base;
using Metoda.Reporting.Common.Enums;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Chart.Plots.Scatter;

public sealed class ScatterChart : ChartBase<ScatterOptions>
{
    private readonly ScatterAxis _xAxis;
    private readonly List<ScatterAxis> _yAxisList;

    private ScatterChart(
        ScatterAxis xAxis,
        List<ScatterAxis> yAxisList,
        string title = null,
        ScatterOptions options = null) : base(ChartType.Scatter, title, options)
    {
        _xAxis = xAxis;
        _yAxisList = yAxisList;
    }

    protected override void FillPlot(Plot plt)
    {
        foreach (ScatterAxis axis in _yAxisList)
        {
            plt.AddScatter(_xAxis.Values, axis.Values, label: axis.Label);
        }

        plt.XLabel(_xAxis.Label ?? string.Empty);
    }

    public static ScatterChart Create(
           ScatterAxis xAxis,
           List<ScatterAxis> yAxisList,
           string title = null,
           ScatterOptions options = null
       )
    {
        if (xAxis is null)
            throw new ArgumentException($"Parameter '{nameof(xAxis)}' must not be null.");

        if (!yAxisList?.Any() ?? true)
            throw new ArgumentException($"Parameter '{nameof(yAxisList)}' must not be null or empty.");

        ValidateAxisLengths(xAxis, yAxisList);

        return new ScatterChart(xAxis, yAxisList, title, options);
    }

    private static void ValidateAxisLengths(ScatterAxis xAxis, List<ScatterAxis> yAxisList)
    {
        int expectedLength = xAxis.Values.Length;

        foreach (var yAxis in yAxisList)
        {
            if (yAxis.Values.Length != expectedLength)
            {
                throw new ArgumentException($"Input arrays [{nameof(xAxis)},{nameof(yAxisList)}.Item] must have the same length.");
            }
        }
    }
}