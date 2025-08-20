using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.ReportElements;
using Metoda.Reporting.Common.Reports.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Metoda.Reporting.Common.Reports;

public abstract class ReportBase<TContainer> : IReport<TContainer>
    where TContainer : class
{
    public IEnumerable<ReportElement<TContainer>> Elems { get; private set; }
    public string Title { get; set; }
    public IReportProgress Progress { get; private set; }

    public ReportBase(IEnumerable<ReportElement<TContainer>> elems, string title, IReportProgress progress = null)
    {
        Elems = elems;
        Title = title;
        Progress = progress;
    }

    public abstract Task ToFileAsync(string dest);

    public abstract void ToFile(string dest);

    public void SetProgress(IReportProgress reportProgress)
    {
        Progress = reportProgress;
    }
}
