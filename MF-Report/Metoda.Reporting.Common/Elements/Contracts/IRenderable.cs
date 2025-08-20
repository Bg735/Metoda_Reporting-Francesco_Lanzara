namespace Metoda.Reporting.Common.Elements.Contracts;

public interface IRenderable<TContainer> 
    where TContainer : class
{
    void Render(TContainer container);
}

public interface IRenderableContentContainer<TContentContainer>
    where TContentContainer : class
{
    void RenderContent(TContentContainer container);
}

public interface IColumnDependentRenderable<TContainer> : IRenderable<TContainer>
    where TContainer : class
{
    void Render(TContainer container, int columnCount);
}
