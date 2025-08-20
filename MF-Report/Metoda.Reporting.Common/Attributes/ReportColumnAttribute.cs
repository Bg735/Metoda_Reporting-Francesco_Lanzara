using Metoda.Reporting.Common.Elements;
using Metoda.Reporting.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Metoda.Reporting.Common.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ReportColumnAttribute : Attribute
{
    public ReportColumn Column { get; }

    public ReportColumnAttribute(
        int order,
        string displayName,
        float colWidth = 1f,
        int colSpan = 1,
        ReportTextHorizontalAlignment textAlignment = ReportTextHorizontalAlignment.CENTER,
        bool isInTotal = false,
        string format = "")
    {
        Column = new ReportColumn(order, displayName, colSpan, colWidth, textAlignment, isInTotal, format);
    }

    public static IList<ReportColumn> GetReportColumns(Type objectType)
    {
        Type type = typeof(ReportColumnAttribute);
        var props = objectType.GetProperties()
            .Where(p => IsDefined(p, type))
            .OrderBy(p => ((ReportColumnAttribute)p.GetCustomAttribute(type)).Column.Order);

        IList<ReportColumn> columns = new List<ReportColumn>();

        foreach (var prop in props)
        {
            var col = ((ReportColumnAttribute)prop.GetCustomAttribute(type)).Column;
            col.PropInfo = prop;
            columns.Add(col);
        }

        return columns;
    }
}