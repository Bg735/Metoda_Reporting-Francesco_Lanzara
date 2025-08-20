using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.ReportElements;
using Metoda.Reporting.Common.Elements.ReportELements;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Common.Reports.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Metoda.Reporting.Common.Builders;

public abstract class ReportBuilderBase<TReport, TContainer>
    where TReport : IReport
    where TContainer : class
{
    protected readonly IList<ReportElement<TContainer>> _elements = new List<ReportElement<TContainer>>();
    public IReportProgress Progress { get; private set; }
    public string ReportTitle { get; private set; }
    public PageOrientation Orientation { get; private set; }

   
    public ReportBuilderBase(string reportTitle, IReportProgress progress, PageOrientation orientation)
    {
        ReportTitle = reportTitle;
        Progress = progress;
        Orientation = orientation;
    }

    public ReportBuilderBase<TReport, TContainer> AddElement(ReportElement<TContainer> element)
    {
        if (element == null)
            throw new ArgumentException(nameof(element));

        _elements.Add(element);
        return this;
    }

    public virtual ReportBuilderBase<TReport, TContainer> AddCompanyLine(ReportCompanyLineBase<TContainer> reportCompanyLine)
    {
        AddElement(reportCompanyLine);
        return this;
    }

    public virtual ReportBuilderBase<TReport, TContainer> AddFilterSection(ReportFilterSectionBase<TContainer> reportFilterSection)
    {
        AddElement(reportFilterSection);
        return this;
    }

    public virtual ReportBuilderBase<TReport, TContainer> AddChart(ReportChartBase<TContainer> reportChart)
    {
        AddElement(reportChart);
        return this;
    }

    public virtual ReportBuilderBase<TReport, TContainer> AddTable<TTableItem, TTable>(
        ReportMultipleTableBase<TTableItem, TTable, TContainer> table,
        IntermediateTotalLocation intermediateTotalLocation)
    where TTableItem : class, IReportTableRowItem
    where TTable : ReportTableBase<TTableItem>
    {
        if (table == null)
            throw new ArgumentException(nameof(table));
        table.IntermediateTableTotalLocation = intermediateTotalLocation;

        table.Progress ??= Progress;

        AddElement(table);
        return this;
    }

    public virtual ReportBuilderBase<TReport, TContainer> AddTable<TTableItem, TTable>(
         ReportMultipleTableBase<TTableItem, TTable, TContainer> table)
        where TTableItem : class, IReportTableRowItem
        where TTable : ReportTableBase<TTableItem>
    {
        table.Progress ??= Progress;

        AddElement(table);
        return this;
    }

    //public virtual ReportBuilderBase<TReport, TElement, TContainer> AddPdfDocumentNestedSection(PdfDocumentNestedSection section)
    //{
    //    AddElement(section);
    //    return this;
    //}

    //public virtual ReportBuilderBase<TReport, TElement, TContainer> AddMainNestedSection(PdfMainNestedSection section)
    //{
    //    AddElement(section);
    //    return this;
    //}

    //public virtual ReportBuilderBase<TReport, TElement, TContainer> AddDetailedSection(PdfDetailedSection section)
    //{
    //    AddElement(section);
    //    return this;
    //}

    public abstract TReport Build();

    protected virtual TReport CreateReport(params object[] args)
    {
        Type type = typeof(TReport);
        Type[] parameterTypes = GetConstuctorParameterTypes();

        ConstructorInfo constructor = type.GetConstructor(parameterTypes);
        return constructor == null ? throw new InvalidOperationException("Constructor not found.") : (TReport)constructor.Invoke(args);
    }

    protected abstract Type[] GetConstuctorParameterTypes();
}