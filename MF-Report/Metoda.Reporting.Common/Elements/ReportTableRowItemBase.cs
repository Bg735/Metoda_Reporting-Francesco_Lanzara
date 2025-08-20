using Metoda.Reporting.Common.Attributes;
using Metoda.Reporting.Common.Elements.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Metoda.Reporting.Common.Elements;

public class ReportTableRowItemBase : IReportTableRowItem
{
    public ValueTuple<int, string>[] GetValueArray(IList<PropertyInfo> itemInfo, CultureInfo ci)
    {
        return itemInfo.Select(_ =>
            new ValueTuple<int, string>(
                (int)((ReportColumnAttribute)_.GetCustomAttribute(typeof(ReportColumnAttribute))).Column.TextAlignment,
                GetValue(this, _, ci))).ToArray();
    }

    public static string GetValue(object obj, PropertyInfo pi, CultureInfo ci)
    {
        string frmt = (string)pi.GetCustomAttribute<ReportColumnAttribute>().Column.Format;

        if (pi.PropertyType == typeof(decimal))
        {
            if (!string.IsNullOrEmpty(frmt))
                return ((decimal)(pi.GetValue(obj) ?? 0M)).ToString(frmt, ci);
            else
                return ((decimal)(pi.GetValue(obj) ?? 0M)).ToString("N2", ci);
        }
        else if ( pi.PropertyType == typeof(decimal?))
        {
            var tmpVal = pi.GetValue(obj);
            if (tmpVal == null)
                return string.Empty;
            else if (!string.IsNullOrEmpty(frmt))

                return ((decimal)tmpVal).ToString(frmt, ci);
            else
                return ((decimal)tmpVal).ToString("N2", ci);
        }
        else if (pi.PropertyType == typeof(int))
        {
          
            if (!string.IsNullOrEmpty(frmt))
                return ((int)(pi.GetValue(obj) ?? 0)).ToString(frmt, ci);
            else
                return ((int)(pi.GetValue(obj) ?? 0)).ToString("N0", ci);
        }
        else if (pi.PropertyType == typeof(int?))
        {
            var tmpVal = pi.GetValue(obj);
            if (tmpVal == null)
                return string.Empty;
            else if (!string.IsNullOrEmpty(frmt))
                return ((int)tmpVal).ToString(frmt, ci);
            else
                return ((int)tmpVal).ToString("N0", ci);
        }
        else if (pi.PropertyType == typeof(long))
        {

            if (!string.IsNullOrEmpty(frmt))
                return ((long)(pi.GetValue(obj) ?? 0)).ToString(frmt, ci);
            else
                return ((long)(pi.GetValue(obj) ?? 0)).ToString("N0", ci);
        }
        else if (pi.PropertyType == typeof(long?))
        {
            var tmpVal = pi.GetValue(obj);
            if (tmpVal == null)
                return string.Empty;
            else if (!string.IsNullOrEmpty(frmt))
                return ((long)tmpVal).ToString(frmt, ci);
            else
                return ((long)tmpVal).ToString("N0", ci);
        }
        else
        {
            return pi.GetValue(obj)?.ToString() ?? string.Empty;
        }
    }
}
