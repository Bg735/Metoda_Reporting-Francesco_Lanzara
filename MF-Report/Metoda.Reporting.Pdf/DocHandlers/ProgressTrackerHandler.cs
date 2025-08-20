using iText.Kernel.Events;
using Metoda.Reporting.Common.Elements.Contracts;
using System;

namespace Metoda.Reporting.Pdf.DocHandlers;

public class ProgressTrackerHandler : IEventHandler
{
    private readonly IReportProgress _progress;

    public ProgressTrackerHandler(IReportProgress progress)
    {
        _progress = progress;
    }

    public void HandleEvent(Event evt)
    {
        if (evt is PdfDocumentEvent docEvent)
        {
            float currentPageNumber = docEvent.GetDocument().GetPageNumber(docEvent.GetPage());
            float totalPages = docEvent.GetDocument().GetNumberOfPages();

            float currentProgress = (1.0f - _progress.InitialValue) * currentPageNumber / totalPages + _progress.InitialValue;

            if (currentProgress > 1.00000001f)
                throw new Exception("IReportProgress.InitialValue should be in range from '0.0f' to '1.0f'");

            _progress.Report(currentProgress);
        }
    }
}
