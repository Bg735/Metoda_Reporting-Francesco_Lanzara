using Metoda.Reporting.Common.Enums;

namespace Metoda.Reporting.Common.Elements.Contracts;

public interface IChart
{
    ChartType ChartType { get; }

    byte[] GetImageAsByteArray();
}