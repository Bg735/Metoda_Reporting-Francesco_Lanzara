namespace Metoda.Reporting.Chart.Plots.BarSeries;

public class BarSerie
{
    public string Label { get; private set; }
    public double Value { get; private set; } = default;
    public double? Position { get; private set; } = default;
    public float LineWidth { get; private set; } = 2;

    public BarSerie(double value, string label = null, double? position = default, float lineWidth = 2) : this(value, label)
    {
        Position = position;
        LineWidth = lineWidth;
    }

    public BarSerie(double value, string label)
    {   
        Label = label;
        Value = value;
    }

    public BarSerie(double value) : this(value, null) { }
}