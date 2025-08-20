using Metoda.Reporting.Common.Attributes;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Enums;

namespace Metoda.Reporting.Models.Reports.ReportingUnlikelyToPayBySubject;

/// <summary>
/// Segnalazione inadempienze probabili per soggetto
/// </summary>
public class ReportingUnlikelyToPayBySubjectItem : ReportTableRowItemBase
{
    [ReportColumn(1, "Cod. Censito", 2f)]
    public string CodCensito { get; set; }

    [ReportColumn(2, "Cubo", 4f, 1, ReportTextHorizontalAlignment.LEFT)]
    public string Cubo { get; set; }

    [ReportColumn(3, "Stato Rapporto", 2f)]
    public string StatoRapporto { get; set; }

    [ReportColumn(4, "Ad Incaglio", 1f, isInTotal: true)]
    public decimal AdIncaglio { get; set; }

    [ReportColumn(5, "Non Incaglio", 1f, isInTotal: true)]
    public decimal NonIncaglio { get; set; }
}
