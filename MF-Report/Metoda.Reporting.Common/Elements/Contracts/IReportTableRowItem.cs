using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Metoda.Reporting.Common.Elements.Contracts;

public interface IReportTableRowItem
{
    (int, string)[] GetValueArray(IList<PropertyInfo> itemInfo, CultureInfo ci);
}