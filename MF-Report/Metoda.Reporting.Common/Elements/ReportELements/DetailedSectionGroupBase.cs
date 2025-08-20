using System.Collections.Generic;

namespace Metoda.Reporting.Common.Elements.ReportElements;

public abstract class DetailedSectionGroupBase<T, TContainer, TContentContainer> :
    SectionContainerBase<IEnumerable<T>, TContainer, TContentContainer>
    where T : DetailedSectionItemBase<TContainer, TContentContainer>
    where TContainer : class
    where TContentContainer : class
{
}
