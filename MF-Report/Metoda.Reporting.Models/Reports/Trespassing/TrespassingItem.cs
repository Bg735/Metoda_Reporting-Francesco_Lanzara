using Metoda.Reporting.Common.Attributes;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Enums;

namespace Metoda.Reporting.Models.Reports.Trespassing;

/// <summary>
/// Sconfinamenti
/// </summary>
public class TrespassingItem : ReportTableRowItemBase
{
    [ReportColumn(1, "Cod. Censito", 2f)]
    public string CodCensito { get; set; }

    [ReportColumn(2, "Cubo", 5f, 1, ReportTextHorizontalAlignment.LEFT)]
    public string Cubo { get; set; }

    [ReportColumn(3, "Accordato", 1f, isInTotal: true)]
    public decimal Accordato { get; set; }

    [ReportColumn(5, "Sbilancio", 1f, isInTotal: true)]
    public decimal Sbilancio { get; set; }

    [ReportColumn(4, "Utilizzato", 1f, isInTotal: true)]
    public decimal Utilizzato { get; set; }
}
