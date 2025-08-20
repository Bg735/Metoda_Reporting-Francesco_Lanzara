using System.Collections.Generic;

namespace Metoda.Reporting.Common.Elements.ReportElements;

public abstract class ColumnSectionItemBase<TContainer, TContentContainer>
    : SectionContainerBase<string[], TContainer, TContentContainer>
    where TContainer : class
    where TContentContainer : class
{
}
