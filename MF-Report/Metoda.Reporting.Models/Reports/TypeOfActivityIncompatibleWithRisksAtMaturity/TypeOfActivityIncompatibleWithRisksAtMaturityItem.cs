using Metoda.Reporting.Common.Attributes;
using Metoda.Reporting.Common.Elements;

namespace Metoda.Reporting.Models.Reports.TypeOfActivityIncompatibleWithRisksAtMaturity;

/// <summary>
/// Tipo attività incompatibile con Rischi a scadenza
/// </summary>
public class TypeOfActivityIncompatibleWithRisksAtMaturityItem : ReportTableRowItemBase
{
    [ReportColumn(1, "Cod. Censito", 2f)]
    public string CodCensito { get; set; }

    [ReportColumn(2, "Fenomeno", 2.5f)]
    public string Fenomeno { get; set; }

    [ReportColumn(3, "Tipo Attività", 2.5f)]
    public string TipoAttivita { get; set; }

    [ReportColumn(4, "Accordato", 1.5f, isInTotal: true)]
    public decimal Accordato { get; set; }

    [ReportColumn(5, "Utilizzato", 1.5f, isInTotal: true)]
    public decimal Utilizzato { get; set; }
}
