using Metoda.Reporting.Common.Attributes;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Enums;

namespace Metoda.Reporting.Models.Reports.PresenceOfLeadPoolAndNotTotalPoolOrViceVersa;

/// <summary>
/// Presenza di pool capofila e non pool totale o viceversa
/// </summary>
public class PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem : ReportTableRowItemBase
{
    [ReportColumn(1, "Cod. Censito", 2f)]
    public string CodCensito { get; set; }

    [ReportColumn(2, "Cubo", 4f, 1, ReportTextHorizontalAlignment.LEFT)]
    public string Cubo { get; set; }

    [ReportColumn(3, "Capofila Accordato", 1f, isInTotal: true)]
    public decimal CapofilaAccordato { get; set; }

    [ReportColumn(4, "Capofila Utilizzato", 1f, isInTotal: true)]
    public decimal CapofilaUtilizzato { get; set; }

    [ReportColumn(5, "Tot. Accordato", 1f, isInTotal: true)]
    public decimal TotAccordato { get; set; }

    [ReportColumn(6, "Tot. Utilizzato", 1f, isInTotal: true)]
    public decimal TotUtilizzato { get; set; }
}
