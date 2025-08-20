using iText.IO.Font;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.ReportElements;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Common.Reports;
using Metoda.Reporting.Common.Res;
using Metoda.Reporting.Pdf.DocHandlers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Metoda.Reporting.Pdf.Reports;

public class PdfReport : ReportBase<Document>
{
    protected readonly PdfFont RegularFont = PdfFontFactory.CreateFont(
        Resource.CALIBRI,
        PdfEncodings.WINANSI,
        PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED
        );

    protected readonly PdfFont BoldFont = PdfFontFactory.CreateFont(
        Resource.CALIBRIB,
        PdfEncodings.WINANSI,
        PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED
        );

    protected readonly PdfFont ItalicFont = PdfFontFactory.CreateFont(
        Resource.CALIBRII,
        PdfEncodings.WINANSI,
        PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED
        );

    public PageOrientation PageOrientation { get; set; }

    public PdfReport(IList<ReportElement<Document>> elems, string title, IReportProgress progress, PageOrientation orientation)
        : base(elems, title, progress)
    {
        PageOrientation = orientation;
    }

    public override Task ToFileAsync(string dest)
    {
        return Task.Run(() =>
             {
                 ToFile(dest);
             });
    }

    public Task ToStreamAsync(Stream dest)
    {
        return Task.Run(() =>
        {
            ToStream(dest);
        });
    }

    public override void ToFile(string dest)
    {
        try
        {
            GenFile(dest);
        }
        catch (FileNotFoundException)
        {
            throw;
        }
        catch (IOException)
        {
            throw;
        }
    }

    public void ToStream(Stream dest)
    {
        try
        {
            GenFile(dest);
        }
        catch (FileNotFoundException)
        {
            throw;
        }
        catch (IOException)
        {
            throw;
        }
    }

    private void GenFile(string dest)
    {
        PdfWriter writer = new(dest);
        PdfDocument pdfDoc = new(writer);
        PageSize pSize =
                PageOrientation == PageOrientation.Portrait
                ? PageSize.A4
                : PageSize.A4.Rotate();

        Document doc = new(pdfDoc, pSize, false);

        if (Progress != null)
        {
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new ProgressTrackerHandler(Progress));
        }

        RenderHeader(pdfDoc, doc);
        Progress?.Report(0.2f);
        RenderBody(doc);
        Progress?.Report(0.9f);
        RenderFooter(pdfDoc);

        doc.Close();
    }

    private void GenFile(Stream dest)
    {
        PdfWriter writer = new(dest);
        PdfDocument pdfDoc = new(writer);
        PageSize pSize =
                PageOrientation == PageOrientation.Portrait
                ? PageSize.A4
                : PageSize.A4.Rotate();

        Document doc = new(pdfDoc, pSize, false);

        if (Progress != null)
        {
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new ProgressTrackerHandler(Progress));
        }

        RenderHeader(pdfDoc, doc);
        Progress?.Report(0.2f);
        RenderBody(doc);
        Progress?.Report(0.9f);
        RenderFooter(pdfDoc);

        doc.Close();
    }

    protected virtual void RenderHeader(PdfDocument pdfDoc, Document doc)
    {
        HeaderEventHandler headerHandler = new(doc, new PdfFont[] { RegularFont, BoldFont, ItalicFont }, Title);
        pdfDoc.AddEventHandler(PdfDocumentEvent.START_PAGE, headerHandler);
    }

    protected virtual void RenderFooter(PdfDocument pdfDoc)
    {
        FooterEventHandler footerHandler = new();
        pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, footerHandler);
        footerHandler.WritePageTotal(pdfDoc);
    }

    protected virtual void RenderBody(Document container)
    {
        float progressEndValue = 0.9f;
        float progressCurrentValue = Progress?.CurrentValue ?? 0F;

        if (Elems != null)
        {
            float progessIncrement = 0f;
            if (Progress != null)
            {
                progessIncrement = (float)((progressEndValue - progressCurrentValue) / Elems.Count());
            }

            foreach (var elem in Elems)
            {
                elem.Render(container);
                progressCurrentValue += progessIncrement;
                Progress?.Report(progressCurrentValue);
            }
        }

        Progress?.Report(progressEndValue);
    }
}