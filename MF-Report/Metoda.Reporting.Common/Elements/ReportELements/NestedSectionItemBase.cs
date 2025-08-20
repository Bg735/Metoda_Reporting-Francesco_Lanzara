using System.Collections.Generic;

namespace Metoda.Reporting.Common.Elements.ReportElements;

public abstract class NestedSectionItemBase<TContainer, TContentContainer>
    : SectionContainerBase<IDictionary<string, string>, TContainer, TContentContainer>
    where TContainer : class
    where TContentContainer : class
{
}
