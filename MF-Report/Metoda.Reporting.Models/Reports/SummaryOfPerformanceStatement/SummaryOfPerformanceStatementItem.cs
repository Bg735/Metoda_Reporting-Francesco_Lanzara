using Metoda.Reporting.Common.Attributes;
using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Enums;

namespace Metoda.Reporting.Models.Reports.SummaryOfPerformanceStatement;

/// <summary>
/// Riepilogo Prospetto Andamentale
/// </summary>
public class SummaryOfPerformanceStatementItem : ReportTableRowItemBase
{
    [ReportColumn(1, "Cubi", 6f, 1, ReportTextHorizontalAlignment.LEFT)]
    public string Cubi { get; set; }

    [ReportColumn(2, "N° Segn. Mese", 1f, isInTotal: true)]
    public int NumSegnMese { get; set; }

    [ReportColumn(3, "Totale Mese", 1f, isInTotal: true)]
    public decimal TotaleMese { get; set; }

    [ReportColumn(4, "N° Segn Mese Prec.", 1f, isInTotal: true)]
    public int NumSegnMesePrec { get; set; }

    [ReportColumn(5, "Totale Mese Prec.", 1f, isInTotal: true)]
    public decimal TotaleMesePrec { get; set; }
}
