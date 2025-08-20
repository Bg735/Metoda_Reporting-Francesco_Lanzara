using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.ReportElements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Metoda.Reporting.Common.Reports.Contracts;

public interface IReport
{
    IReportProgress Progress { get; }
    string Title { get; set; }

    Task ToFileAsync(string dest);
    void ToFile(string dest);
    void SetProgress(IReportProgress reportProgress);
}

public interface IReport<TContainer>: IReport
    where TContainer : class
{
    IEnumerable<ReportElement<TContainer>> Elems { get; }
}