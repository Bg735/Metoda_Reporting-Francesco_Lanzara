using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.ReportElements;

namespace Metoda.Reporting.Common.Elements.ReportELements
{
    public abstract class ReportChartBase<TContainer> : ReportElement<TContainer> where TContainer : class
    {
        protected readonly IChart _chart;

        public ReportChartBase(IChart chart)
        {
            _chart = chart;
        }

        public byte[] GetChartByteArray()
        {
            return null; // GetImageAsByteArray(chartType);
        }
    }
}