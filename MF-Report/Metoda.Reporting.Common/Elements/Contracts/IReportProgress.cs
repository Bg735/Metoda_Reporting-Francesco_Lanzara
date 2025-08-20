using System;

namespace Metoda.Reporting.Common.Elements.Contracts;

public interface IReportProgress : IProgress<float>
{
    float InitialValue { get; }
    float CurrentValue { get; }
}
