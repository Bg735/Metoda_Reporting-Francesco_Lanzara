using Metoda.Reporting.Chart.Plots.Base;
using Metoda.Reporting.Common.Attributes;
using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Chart.Plots.Pie;

public sealed class PieChart : ChartBase<PieOptions>
{
    private readonly double[] _values;
    private readonly string[] _labels;

    private PieChart(string[] labels, double[] values, string title = null, PieOptions options = null) : base(ChartType.Pie, title, options)
    {
        _labels = labels;
        _values = values;
    }

    protected override void FillPlot(Plot plt)
    {
        var pie = plt.AddPie(_values);
        pie.SliceLabels = _labels;

        pie.Explode = Options.Explode;
        pie.DonutSize = Options.DonutSize;
        pie.ShowValues = Options.ShowValues;
        pie.ShowPercentages = Options.ShowPercentages;
        pie.Size = Options.SizeScale;    
    }

    public static PieChart Create<T>(TotalRow<T> total, string[] props = null, string title = null, PieOptions options = null)
        where T : class, IReportTableRowItem
    {
        if (total is null)
            throw new ArgumentException(nameof(total));

        if (props is not null && props.Length == 0)
            throw new ArgumentException($"Parameter '{nameof(props)}' must not be null or empty.");

        var columns = ReportColumnAttribute
            .GetReportColumns(typeof(T))
            .Where(_ => _.IsInTotal && (props is null || props.Contains(_.PropInfo.Name))
            );

        if (!columns?.Any() ?? true)
            throw new ArgumentException($"Parameter '{nameof(props)}' does not contain any valid value.");

        var labels = columns.Select(_ => _.DisplayName).ToArray();
        var values = columns
            .Select(_ => GetPropertyValueConvertedToDouble(_.PropInfo.GetValue(total.Row)))
            .ToArray();

        return new PieChart(labels, values, title, options);
    }

    public static PieChart Create<T>(IEnumerable<TotalRow<T>> totals, string valuePropertyName, string title = null, PieOptions options = null)
        where T : class, IReportTableRowItem
    {
        if (totals is null || !totals.Any())
            throw new ArgumentException($"Parameter '{nameof(totals)}' must not be null or empty");

        if (string.IsNullOrWhiteSpace(valuePropertyName))
            throw new ArgumentException($"Parameter '{nameof(valuePropertyName)}' must not be null or empty.");

        var column = ReportColumnAttribute.GetReportColumns(typeof(T))
           .FirstOrDefault(_ => _.IsInTotal && _.PropInfo.Name == valuePropertyName);

        if (column is null)
            throw new ArgumentException($"Parameter '{nameof(valuePropertyName)}' is not valid");

        var labels = totals.Select(_ => $"{_.Label}({valuePropertyName})").ToArray();

        var values = totals.Select(_ => GetPropertyValueConvertedToDouble(column.PropInfo.GetValue(_.Row))).ToArray();

        return new PieChart(labels, values, title, options);
    }

    public static PieChart Create(List<Pie> pies, string title = null, PieOptions options = null)
    {
        if (pies is null || !pies.Any())
            throw new ArgumentException($"Parameter '{nameof(pies)}' must not be null or empty");

        return new PieChart(
            pies.Select(p => p.Label).ToArray(), 
            pies.Select(p => p.Value).ToArray(), 
            title, 
            options);
    }
}
