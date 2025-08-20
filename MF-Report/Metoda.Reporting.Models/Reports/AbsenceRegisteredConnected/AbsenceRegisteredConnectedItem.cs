using Metoda.Reporting.Common.Attributes;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Enums;

namespace Metoda.Reporting.Models.Reports.AbsenceRegisteredConnected;

/// <summary>
/// Assenza Censito Collegato
/// </summary>
public class AbsenceRegisteredConnectedItem : ReportTableRowItemBase
{
    [ReportColumn (2, "Cod.Censito", 2f)]
    public string CodCensito { get; set; }

    [ReportColumn(3, "Fenomeno", 4f, 1, ReportTextHorizontalAlignment.LEFT)]
    public string Fenomeno { get; set; }

    [ReportColumn(4, "Stato Rapporto", 1.5f)]
    public string StatoRapporto { get; set; }

    [ReportColumn(5, "Valore", 1.5f, isInTotal: true)]
    public decimal Valore { get; set; }
}
