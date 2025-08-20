using Metoda.Reporting.Common.Attributes;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Enums;

namespace Metoda.Reporting.Models.Reports.InconsistencyBetweenOriginalAndResidualDuration;

/// <summary>
/// Incongruenza tra durata originaria e residua
/// </summary>
public class InconsistencyBetweenOriginalAndResidualDurationItem : ReportTableRowItemBase
{
    [ReportColumn(1, "Cod.Censito", 1.5f)]
    public string CodCensito { get; set; }

    [ReportColumn(2, "Cubo", 2.5f, 1, ReportTextHorizontalAlignment.LEFT)]
    public string Cubo { get; set; }

    [ReportColumn(3, "Durata Originaria", 1.5f)]
    public string DurataOriginaria { get; set; }

    [ReportColumn(4, "Durata Residua", 1.5f)]
    public string DurataResidua { get; set; }

    [ReportColumn(5, "Accordato", 1f, isInTotal: true)]
    public decimal Accordato { get; set; }

    [ReportColumn(6, "Utilizzato", 1f, isInTotal: true)]
    public decimal Utilizzato { get; set; }

    [ReportColumn(7, "Sbilancio", 1f, isInTotal: true)]
    public decimal Sbilancio { get; set; }
}
