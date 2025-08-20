using Metoda.Reporting.Chart.Plots.Pie;
using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Pdf.Builders;
using Metoda.Reporting.Pdf.ReportElements;
using Metoda.Reporting.Pdf.Reports;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Metoda.Reporting.Tests.UnitTests.Pdfs.Charts
{
    [TestFixture]
    public class PieChartTest
    {
        private PdfReportBuilder<PdfReport> _builder;
        private string _folderPath = @"./TestReports/Pies";
        private string _outputFilePathFmt;

        [SetUp]
        public void SetUp()
        {
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

            var progress = new Mock<IReportProgress>();
            progress.Setup(p => p.InitialValue).Returns(0.0f);
            progress.Setup(p => p.CurrentValue).Returns(50.0f);

            _builder = new PdfReportBuilder<PdfReport>("Pie Chart Sample", progress.Object, PageOrientation.Portrait);
            _outputFilePathFmt = Path.Combine(_folderPath, $"{{0}}-Chart-{DateTime.Now:yyyy-MM-dd__HH_mm_ss}.pdf");
        }

        //[TearDown]
        //public void TearDown()
        //{
        //    string[] files = Directory.GetFiles(_folderPath);
            
        //    foreach (string file in files)
        //    {
        //        File.Delete(file);
        //    }
        //}

        [Test]
        public void Create_PieChartWithValidPies_ReturnsPieChart()
        {
            // Arrange

            var pies = new List<Pie> {

                    new Pie(10.62, "Item 1"),
                    new Pie(34.67, "Item 2"),
                    new Pie(15.32, "Item 3"),
                    new Pie(40.13, "Item 4"),
                    new Pie(4.79, "Item 5"),
                };

            var options = new PieOptions
            {
                DonutSize = 0.5,
                SizeScale = 0.8,
                ShowValues = true,
            };

            var filePath = string.Format(_outputFilePathFmt, "Simple-Pie");

            // Act

            var pieChart = PieChart.Create(pies, "Simple Pie chart test", options);
            _builder.AddChart(new PdfReportChart(pieChart));

            var report = _builder.Build();
            report.ToFile(filePath);

            var byteArray = pieChart.GetImageAsByteArray();
            // Assert
            Assert.IsNotNull(pieChart);
            Assert.IsNotNull(byteArray);
            CollectionAssert.IsNotEmpty(byteArray);
            Assert.IsTrue(File.Exists(filePath));
        }

        [Test]
        public void Create_PercentagePieChartWithValidPies_ReturnsPieChart()
        {
            // Arrange

            var pies = new List<Pie> {

                    new Pie(10.62, "Item 1"),
                    new Pie(34.67, "Item 2"),
                    new Pie(15.32, "Item 3"),
                    new Pie(40.13, "Item 4"),
                    new Pie(4.79, "Item 5"),
                };

            var options = new PieOptions
            {
                DonutSize = 0.3,
                SizeScale = 0.8,
                ShowValues = false,
                ShowPercentages = true
            };

            var filePath = string.Format(_outputFilePathFmt, "Percentage-Pie");

            // Act

            var pieChart = PieChart.Create(pies, "Percentage Pie chart test", options);
            _builder.AddChart(new PdfReportChart(pieChart));

            var report = _builder.Build();
            report.ToFile(filePath);

            var byteArray = pieChart.GetImageAsByteArray();
            // Assert
            Assert.IsNotNull(pieChart);
            Assert.IsNotNull(byteArray);
            CollectionAssert.IsNotEmpty(byteArray);
            Assert.IsTrue(File.Exists(filePath));
        }

        [Test]
        public void Create_PieChartWithEmptyProps_ThrowsArgumentException()
        {
            // Act and Assert
            Assert.Throws<ArgumentException>(() => PieChart.Create(null));
        }
    }
}