using Metoda.Reporting.Common.Attributes;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Enums;

namespace Metoda.Reporting.Models.Reports.SufferingsReportedWithOtherPhenomena;

/// <summary>
/// Sofferenze segnalate con altri fenomeni
/// </summary>
public class SufferingsReportedWithOtherPhenomenaItem : ReportTableRowItemBase
{
    [ReportColumn(1, "Cod.Censito", 2f)]
    public string CodCensito { get; set; }

    [ReportColumn(2, "Stato Rapporto", 2f)]
    public string StatoRapporto { get; set; }

    [ReportColumn(3, "Sofferenza", 1.0f, isInTotal: true)]
    public decimal Sofferenza { get; set; }

    [ReportColumn(4, "Cubo 2", 3.0f, 1, ReportTextHorizontalAlignment.LEFT)]
    public string Cubo2 { get; set; }

    [ReportColumn(5, "Altro Cubo", 1.0f, isInTotal: true)]
    public decimal AltroCubo { get; set; }

    [ReportColumn(6, "Totale", 1.0f, isInTotal: true)]
    public decimal Totale { get; set; }
}
