using Metoda.Reporting.Common.Attributes;
using Metoda.Reporting.Common.Elements;

namespace Metoda.Reporting.Models.Reports.GuaranteesConnectedWithOperationsOfACommercialNature;

/// <summary>
/// Garanzie connesse con operazioni di natura commerciale
/// </summary>
public class GuaranteesConnectedWithOperationsOfACommercialNatureItem : ReportTableRowItemBase
{
    [ReportColumn(1, "Cod.Censito", 2f)]
    public string CodCensito { get; set; }

    [ReportColumn(2, "Localizzazione", 2f)]
    public string Localizzazione { get; set; }

    [ReportColumn(3, "Divisa", 1f)]
    public string Divisa { get; set; }

    [ReportColumn(4, "Import/Export", 2f)]
    public string ImportOrExport { get; set; }

    [ReportColumn(5, "StatoRapporto", 2f)]
    public string StatoRapporto { get; set; }

    [ReportColumn(5, "Utilizzato", 1.5f, isInTotal: true)]
    public decimal Utilizzato { get; set; }
}
