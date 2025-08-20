using Metoda.Reporting.Common.Enums;
using ScottPlot;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Chart.Plots.Base;

public abstract class BarChartBase<T> : ChartBase<T>
    where T: BarOptionsBase, new()
{
    protected BarChartBase(ChartType chartType, string title = null, T options = null)
        : base(chartType, title, options)
    {
    }

    protected override void FillPlot(Plot plt)
    {
        var vals = UpdatePlot(plt);

        plt.SetAxisLimits(
            xMin: Options.XAxisLimitMin,
            xMax: Options.XAxisLimitMax,
            yMin: Options.YAxisLimitMin ?? vals.Min() * 0.5,
            yMax: Options.YAxisLimitMax ?? vals.Max() * 1.2
            );

        UpdatePlot(plt);

    }

    protected abstract IEnumerable<double> UpdatePlot(Plot plt);

}