using Metoda.Reporting.Common.Builders;
using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.ReportElements;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Excel.Reports;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;

namespace Metoda.Reporting.Excel.Builders;

public class ExcelReportBuilder<TReport> : ReportBuilderBase<TReport, ISheet>
    where TReport : ExcelReport
{
    public ExcelReportBuilder(string reportTitle, IReportProgress progress, PageOrientation orientation)
        : base(reportTitle, progress, orientation)
    {
    }

    public ExcelReportBuilder<TReport> AddDocumentNestedSection(ExcelDocumentNestedSection section)
    {
        if (section == null)
            throw new ArgumentException(nameof(section));

        AddElement(section);
        return this;
    }

    public ExcelReportBuilder<TReport> AddMainNestedSection(ExcelMainNestedSection mainNestedSection)
    {
        if (mainNestedSection == null)
            throw new ArgumentException(nameof(mainNestedSection));

        AddElement(mainNestedSection);
        return this;
    }

    public ExcelReportBuilder<TReport> AddDetailedSection(ExcelDetailedSection detailedSection)
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
                    typeof(IList<ReportElement<ISheet>>),
                    typeof(string),
                    typeof(IReportProgress),
                    typeof(PageOrientation)
                };
    }
}
