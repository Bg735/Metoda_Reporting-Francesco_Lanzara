namespace Metoda.Reporting.Common.Elements.ReportElements;

public abstract class ReportFilterSectionBase<TContainer> : ReportElement<TContainer>
    where TContainer : class
{
    public float FontSize { get; set; } = 11f;
    public string Label { get; set; } = "My Filter";
    public string Content { get; set; } = "<Filter content is not defined>";

    public ReportFilterSectionBase()
    {
    }

    public ReportFilterSectionBase(string label, string content)
    {
        Label = label;
        Content = content;
    }
}
