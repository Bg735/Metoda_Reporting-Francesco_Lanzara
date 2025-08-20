using Metoda.Reporting.Common.Attributes;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Enums;

namespace Metoda.Reporting.Models.Reports.OtherCreditsButImpaired;

/// <summary>
/// Altri Crediti ma Deteriorati
/// </summary>
public class OtherCreditsButImpairedItem : ReportTableRowItemBase
{
    [ReportColumn(1, "Cod. Censito", 2f)]
    public string CodCensito { get; set; }

    [ReportColumn(2, "Cubo", 4f, 1, ReportTextHorizontalAlignment.LEFT)]
    public string Cubo { get; set; }

    [ReportColumn(3, "Stato Rapporto", 1.5f)]
    public string StatoRapporto { get; set; }

    [ReportColumn(4, "Qualità Credito", 1.5f)]
    public string QualitaCredito { get; set; }

    [ReportColumn(5, "Utilizzato", 1f, isInTotal: true)]
    public decimal Utilizzato { get; set; }
}
