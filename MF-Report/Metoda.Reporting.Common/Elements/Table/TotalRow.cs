using Metoda.Reporting.Common.Elements.Contracts;

namespace Metoda.Reporting.Common.Elements.Table;

public class TotalRow<T>
   where T : class, IReportTableRowItem
{
    public T Row { get; private set; }
    public string Label { get; private set; }

    public TotalRow(T row, string label = "Totale")
    {
        Row = row;
        Label = label;
    }
}
