using Metoda.Reporting.Common.Enums;
using System.Collections.Generic;
using System.Reflection;

namespace Metoda.Reporting.Common.Elements.Contracts
{
    public interface IReportMultipleTable
    {
        IntermediateTotalLocation IntermediateTableTotalLocation { get; set; }
        IReportProgress Progress { get; set; }
        string Title { get; set; }
        IList<ReportColumn> Columns { get; }
        public IList<PropertyInfo> PropInfos { get; }
    }
}