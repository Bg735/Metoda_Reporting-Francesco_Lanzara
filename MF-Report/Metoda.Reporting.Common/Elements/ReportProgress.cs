using Metoda.Reporting.Common.Elements.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace Metoda.Reporting.Common.Elements;

public class ReportProgress : Progress<float>, IReportProgress
{
    [Range(0.0f, 1.0f)]
    public float InitialValue { get; private set; }

    [Range(0.0f, 1.0f)]
    public float CurrentValue { get; private set; }

    public ReportProgress(Action<float> action, float initialValue = 0.0f) : base(action)
    {
        InitialValue = initialValue;
    }

    public void SetCurrentValue(float value)
    {
        CurrentValue = value;
    }

    protected override void OnReport(float value)
    {
        CurrentValue = value;
        base.OnReport(value);
    }
}
