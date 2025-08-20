using Metoda.Reporting.Common.Attributes;
using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Metoda.Reporting.Common.Elements;

public abstract class ReportTableBase<T> : IReportTable<T> where T : class, IReportTableRowItem
{
    public IList<PropertyInfo> PropInfos { get; protected set; }
    public IList<ReportColumn> Columns { get; private set; }
    public IList<T> Rows { get; private set; }
    public TotalRow<T> TotalRow { get; set; }

    public ReportTableBase(IList<T> rows = null, TotalRow<T> totalRow = null)
    {
        Rows = rows;
        TotalRow = totalRow;
        Columns = ReportColumnAttribute.GetReportColumns(typeof(T));
        PropInfos = Columns.Select(_ => _.PropInfo).ToList();
    }
}
