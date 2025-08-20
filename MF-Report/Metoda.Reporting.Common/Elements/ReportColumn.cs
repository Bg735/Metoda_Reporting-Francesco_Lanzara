using Metoda.Reporting.Common.Enums;
using System.Reflection;

namespace Metoda.Reporting.Common.Elements;

public class ReportColumn
{
    public int Order { get; }

    public string DisplayName { get; }

    /// <summary>
    /// Larghezza Colonna
    /// </summary>
    /// <remarks>Default value 1f </remarks>
    public float ColWidth { get; }

    /// <summary>
    /// Numero di colonne da occupare
    /// </summary>
    /// <remarks>Default value 1 </remarks>
    public int ColSpan { get; }

    public ReportTextHorizontalAlignment TextAlignment { get; }

    public bool IsInTotal { get; }

    public PropertyInfo PropInfo { get; set; }

    /// <summary>
    /// Opzionale: definisce la formattazione in output per i tipi numerici
    /// </summary>
    public string Format { get; set; }

    public ReportColumn(
        int order,
        string displayName,
        int colSpan = 1,
        float colWidth = 1f,
        ReportTextHorizontalAlignment textAlignment = ReportTextHorizontalAlignment.CENTER,
        bool isInTotal = false,
        string format = ""
        )
    {
        Order = order;
        DisplayName = displayName;
        ColSpan = colSpan;
        ColWidth = colWidth;
        TextAlignment = textAlignment;
        IsInTotal = isInTotal;
        Format = format;
    }
}
