using Metoda.Reporting.Common.Elements.Table;
using System.Collections.Generic;
using System.Reflection;

namespace Metoda.Reporting.Common.Elements.Contracts;

public interface IReportTable<T> where T : class, IReportTableRowItem
{
    IList<ReportColumn> Columns { get; }
    IList<PropertyInfo> PropInfos { get; }
    IList<T> Rows { get; }
    TotalRow<T> TotalRow { get; set; }
}