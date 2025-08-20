using Metoda.Reporting.Chart.Plots.Base;
using Metoda.Reporting.Common.Enums;
using ScottPlot;
using System;

namespace Metoda.Reporting.Chart.Plots.Base64Image
{
    public sealed class Base64Image : ChartBase<Base64ImageOptions>
    {
        private readonly string _base64Image;
        private Base64Image(
            string base64Image, 
            string title = null,
            Base64ImageOptions options = null) 
            : base(ChartType.Base64Image, title, options)
        {
            _base64Image = base64Image;
        }

        protected override void FillPlot(Plot plt)
        {
        }

        public override byte[] GetImageAsByteArray()
        {
            return Convert.FromBase64String(_base64Image);
        }

        public static Base64Image Create(string base64Image)
        {
            return new Base64Image(base64Image);
        }
    }
}
