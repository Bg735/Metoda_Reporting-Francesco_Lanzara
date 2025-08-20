using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Contracts.Sections;

namespace Metoda.Reporting.Common.Elements.ReportElements;

public abstract class SectionContainerBase<T, TContainer, TContentContainer> 
    : ReportElement<TContainer>, 
        ISectionContainer<T>
        where T : class
        where TContainer : class
        where TContentContainer : class
{
    public string Title { get; set; }

    public string TitleEnd { get; set; } = string.Empty;

    public T Content { get; set; }


}
