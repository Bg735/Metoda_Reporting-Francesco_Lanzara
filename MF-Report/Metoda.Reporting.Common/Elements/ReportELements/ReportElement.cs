using Metoda.Reporting.Common.Elements.Contracts;
using System;

namespace Metoda.Reporting.Common.Elements.ReportElements;

public abstract class ReportElement
    <TContainer> : IColumnDependentRenderable<TContainer>
   where TContainer : class
{
    public virtual void Render(TContainer container)
    {
        throw new NotImplementedException();
    }

    public virtual void Render(TContainer container, int columnCount)
    {
        throw new NotImplementedException();
    }
}
