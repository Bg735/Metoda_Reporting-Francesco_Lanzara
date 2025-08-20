using Metoda.Reporting.Common.Attributes;
using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.ReportElements;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Metoda.Reporting.Common.Elements;

public abstract class ReportMultipleTableBase<T, TTable, TContainer> : ReportElement<TContainer>, IReportMultipleTable where TContainer : class
    where TTable : ReportTableBase<T>
    where T : class, IReportTableRowItem
{
    public IntermediateTotalLocation IntermediateTableTotalLocation { get; set; }

    public IList<PropertyInfo> PropInfos {get; protected set;}
    public IList<ReportColumn> Columns { get; }
    public IList<TTable> Tables { get; private set; }
    public TotalRow<T> MainTotalRow { get; set; }
    public string Title { get; set; }

    public IReportProgress Progress { get; set; }

    public ReportMultipleTableBase(IList<TTable> tables, TotalRow<T> mainTotalRow, string title = null, IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom)
    {
        IntermediateTableTotalLocation = totalLocation;
        Tables = tables;
        MainTotalRow = mainTotalRow;
        Title = title;
        Columns = ReportColumnAttribute.GetReportColumns(typeof(T));
        PropInfos = Columns.Select(_ => _.PropInfo).ToList();
    }
}
