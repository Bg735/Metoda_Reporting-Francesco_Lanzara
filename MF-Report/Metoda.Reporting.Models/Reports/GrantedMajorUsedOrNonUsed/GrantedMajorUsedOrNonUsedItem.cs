using Metoda.Reporting.Common.Attributes;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Enums;

namespace Metoda.Reporting.Models.Reports.GrantedMajorUsedOrNonUsed;

/// <summary>
/// Accordato Magg Utilizzato Osenza Utilizzato
/// </summary>
public class GrantedMajorUsedOrNonUsedItem : ReportTableRowItemBase
{
    [ReportColumn(1, "Cod. Censito", 2f)]
    public string CodCensito { get; set; }

    [ReportColumn(3, "Cubo", 5f, 1, ReportTextHorizontalAlignment.LEFT)]
    public string Cubo { get; set; }

    [ReportColumn(4, "Accordato", 1f, isInTotal: true)]
    public decimal Accordato { get; set; }

    [ReportColumn(6, "Sbilancio", 1f, isInTotal: true)]
    public decimal Sbilancio { get; set; }

    [ReportColumn(5, "Utilizzato", 1f, isInTotal: true)]
    public decimal Utilizzato { get; set; }

    [ReportColumn(2, "NDG", 1f, 1, ReportTextHorizontalAlignment.LEFT)]
    public string NDG { get; set; }
}
