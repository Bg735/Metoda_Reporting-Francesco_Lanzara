using iText.Layout;
using Metoda.Reporting.Common.Builders;
using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.ReportElements;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Pdf.ReportElements;
using Metoda.Reporting.Pdf.Reports;
using System;
using System.Collections.Generic;

namespace Metoda.Reporting.Pdf.Builders;

public class PdfReportBuilder<TReport>
    : ReportBuilderBase<TReport, Document>
    where TReport : PdfReport
{
    public PdfReportBuilder(string reportTitle, IReportProgress progress, PageOrientation orientation)
        : base(reportTitle, progress, orientation)
    {
    }

    public PdfReportBuilder<TReport> AddDocumentNestedSection(PdfDocumentNestedSection section)
    {
        if (section == null)
            throw new ArgumentException(nameof(section));

        AddElement(section);
        return this;
    }

    public PdfReportBuilder<TReport> AddMainNestedSection(PdfMainNestedSection mainNestedSection)
    {
        if (mainNestedSection == null)
            throw new ArgumentException(nameof(mainNestedSection));

        AddElement(mainNestedSection);
        return this;
    }

    public PdfReportBuilder<TReport> AddColumnSection(PdfColumnSection columnSection)
    {
        if (columnSection == null)
            throw new ArgumentException(nameof(columnSection));

        AddElement(columnSection);
        return this;
    }

    public PdfReportBuilder<TReport> AddDetailedSection(PdfDetailedSection detailedSection)
    {
        if (detailedSection == null)
            throw new ArgumentException(nameof(detailedSection));

        AddElement(detailedSection);
        return this;
    }

    public override TReport Build()
    {
        return CreateReport(_elements, ReportTitle, Progress, Orientation);
    }

    protected override Type[] GetConstuctorParameterTypes()
    {
        return new[]
            {
                typeof(IList<ReportElement<Document>>),
                typeof(string),
                typeof(IReportProgress),
                typeof(PageOrientation) 
            };
    }
}
