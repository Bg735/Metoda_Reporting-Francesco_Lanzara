using System.Collections.Generic;

namespace Metoda.Reporting.Common.Elements.ReportElements;

public abstract class DetailedSectionItemBase<TContainer, TContentContainer>
    : SectionContainerBase<IDictionary<string, string>, TContainer, TContentContainer>
    where TContainer : class
    where TContentContainer : class
{
    public string KeyHeader { get; set; } = "Campo";
    public string ValueHeader { get; set; } = "Valore";

}
