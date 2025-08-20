using Metoda.Reporting.Common.Attributes;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Enums;

namespace Metoda.Reporting.Models.Reports.OperationalOverruns;


/// <summary>
/// Sconfinamenti Operativi
/// </summary>
public abstract class OperationalOverrunsItem : ReportTableRowItemBase
{
    [ReportColumn(1, "Fenomeno", 1f)]
    public string Fenomeno { get; set; }

    [ReportColumn(2, "Descrizione", 2f, 1, ReportTextHorizontalAlignment.LEFT)]
    public string Descrizione { get; set; }

    [ReportColumn(5, "Utilizzato (33)", 1f)]
    public decimal Utilizzato_33 { get; set; }

    [ReportColumn(6, "Accordato (31)", 1f)]
    public decimal Accordato_31 { get; set; }

    [ReportColumn(7, "Acc.Operativo (32)", 1f)]
    public decimal AccOperativo_32 { get; set; }

    [ReportColumn(8, "Sconf.Deliberato (33-31)", 1f)]
    public decimal SconfDeliberato_33_31 { get; set; }

    [ReportColumn(9, "Sconf.Operativo (33-32)", 1f)]
    public decimal SconfOperativo_33_32 { get; set; }
}

public class OperationalOverrunsSinteticsItem : OperationalOverrunsItem
{
}

public class OperationalOverrunsAnaliticsItem : OperationalOverrunsItem
{
    [ReportColumn(3, "Cod. Censito", 1f)]
    public string CodCensito { get; set; }

    [ReportColumn(4, "NDG", 1f)]
    public string NDG { get; set; }
}
