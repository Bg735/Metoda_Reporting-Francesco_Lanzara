namespace Metoda.Reporting.Common.Elements.Contracts.Sections;

public interface ISection
{
    string Title { get; set; }
    string TitleEnd { get; set; }
}

public interface ISectionContainer<T> : ISection
    where T : class
{
    T Content { get; set; }
}