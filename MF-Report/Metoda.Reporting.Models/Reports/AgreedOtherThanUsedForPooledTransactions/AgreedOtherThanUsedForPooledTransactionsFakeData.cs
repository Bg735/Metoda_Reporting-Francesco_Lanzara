using Metoda.Reporting.Chart.Plots.Pie;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Models.Reports.AgreedOtherThanUsedForPooledTransactions;

public static class AgreedOtherThanUsedForPooledTransactionsFakeData
{
    private static readonly string[] _centsitos = new[] { " - Soggetto A", " - Soggetto B", " - Soggetto C" };

    private static void FillSubtables(
        out List<AgreedOtherThanUsedForPooledTransactionsItem> subTable,
        out TotalRow<AgreedOtherThanUsedForPooledTransactionsItem> totalRow,
        int counter)
    {
        Random random = new();
        decimal accordato, utilizzato;

        subTable = new List<AgreedOtherThanUsedForPooledTransactionsItem>();

        for (int i = 1; i <= 3; i++)
        {
            accordato = random.Next(4000, 10000);
            utilizzato = random.Next(1000, 4000);
            subTable.Add(new AgreedOtherThanUsedForPooledTransactionsItem
            {
                Accordato = accordato,
                CodCensito = $"{50_000 * counter + i}{_centsitos[counter]}",
                Cubo = $"{random.Next(554900, 554902)} - Sezione Informativa – Crediti per Cassa: Operazioni in pool – Azienda capofila",
                Utilizzato = utilizzato,
                Sbilancio = accordato - utilizzato
            });
        }

        accordato = subTable.Select(_ => _.Accordato).Sum();
        utilizzato = subTable.Select(_ => _.Utilizzato).Sum();

        totalRow = new TotalRow<AgreedOtherThanUsedForPooledTransactionsItem>(
            new AgreedOtherThanUsedForPooledTransactionsItem
            {
                Accordato = accordato,
                Utilizzato = utilizzato,
                Sbilancio = accordato - utilizzato
            }, $"Totale cubo {_centsitos[counter]} per n° {subTable.Count} Forme Tecniche");
    }

    public static IList<ExcelTable<AgreedOtherThanUsedForPooledTransactionsItem>> GetExcelTable()
    {
        var list = new List<ExcelTable<AgreedOtherThanUsedForPooledTransactionsItem>>();

        for (int k = 0; k < _centsitos.Length; k++)
        {
            FillSubtables(out List<AgreedOtherThanUsedForPooledTransactionsItem> subTable, out TotalRow<AgreedOtherThanUsedForPooledTransactionsItem> totalRow, k);

            var res = new ExcelTable<AgreedOtherThanUsedForPooledTransactionsItem>(subTable, totalRow);

            list.Add(res);
        }

        return list;
    }

    public static IList<PdfTable<AgreedOtherThanUsedForPooledTransactionsItem>> GetPdfTable()
    {
        var list = new List<PdfTable<AgreedOtherThanUsedForPooledTransactionsItem>>();

        for (int k = 0; k < _centsitos.Length; k++)
        {
            FillSubtables(out List<AgreedOtherThanUsedForPooledTransactionsItem> subTable, out TotalRow<AgreedOtherThanUsedForPooledTransactionsItem> totalRow, k);

            var res = new PdfTable<AgreedOtherThanUsedForPooledTransactionsItem>(subTable, totalRow);

            list.Add(res);
        }

        return list;
    }

    public static void FillBuilderByData(AgreedOtherThanUsedForPooledTransactionsPdfReportBuilder builder)
    {
        var companyLine = new PdfReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var filterSection = new PdfReportFilterSection("Filter", "Cubo = 50200 AND Utilizzato > 10.000");

        var subTables = GetPdfTable();

        var tableTotals = subTables.Select(_ => _.TotalRow);
        decimal accordato = tableTotals.Select(_ => _.Row?.Accordato ?? 0).Sum();
        decimal sbilancio = tableTotals.Select(_ => _.Row?.Sbilancio ?? 0).Sum();

        int totalCount = subTables.Select(_ => _.Rows?.Count ?? 0).Sum();

        var mainTotal = new TotalRow<AgreedOtherThanUsedForPooledTransactionsItem>(
            new AgreedOtherThanUsedForPooledTransactionsItem
            {
                Accordato = accordato,
                Sbilancio = sbilancio,
                Utilizzato = accordato - sbilancio
            },
            $"Totale cubo ALL OF POSSIBLE per n° {totalCount} Forme Tecniche"
        );

        var table = new AgreedOtherThanUsedForPooledTransactionsPdfReportTable(subTables, mainTotal);

        var pieChart = PieChart.Create<AgreedOtherThanUsedForPooledTransactionsItem>(
            mainTotal,
            new[] {
                nameof(AgreedOtherThanUsedForPooledTransactionsItem.Sbilancio),
                nameof(AgreedOtherThanUsedForPooledTransactionsItem.Utilizzato)
            },
            mainTotal.Label);

        var chart = new PdfReportChart(pieChart);

        var varAndLabel = nameof(AgreedOtherThanUsedForPooledTransactionsItem.Accordato);

        var pieChart2_0 = PieChart.Create<AgreedOtherThanUsedForPooledTransactionsItem>(
            tableTotals,
            varAndLabel,
            varAndLabel
            );
        
        var chart2_0 = new PdfReportChart(pieChart2_0);

        varAndLabel = nameof(AgreedOtherThanUsedForPooledTransactionsItem.Utilizzato);

        var pieChart2_1 = PieChart.Create<AgreedOtherThanUsedForPooledTransactionsItem>(
            tableTotals,
            varAndLabel,
            varAndLabel
            );

        var chart2_1 = new PdfReportChart(pieChart2_1);

        varAndLabel = nameof(AgreedOtherThanUsedForPooledTransactionsItem.Sbilancio);

        var pieChart2_2 = PieChart.Create<AgreedOtherThanUsedForPooledTransactionsItem>(
            tableTotals,
            varAndLabel,
            varAndLabel
            );

        var chart2_2 = new PdfReportChart(pieChart2_2);


        builder.AddCompanyLine(companyLine);
        builder.AddFilterSection(filterSection);
        builder.AddTable(table);

        builder.AddChart(chart)
                .AddChart(chart2_0)
                .AddChart(chart2_1)
                .AddChart(chart2_2);
    }

    public static void FillBuilderByData(AgreedOtherThanUsedForPooledTransactionsExcelReportBuilder builder)
    {
        var companyLine = new ExcelReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var filterSection = new ExcelReportFilterSection("Filter",
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Labore molestiae ipsam nemo iure! Recusandae nulla, fugiat ad voluptatibus impedit similique laboriosam tenetur alias! Sunt magni porro veritatis quos, laborum fugiat.");

        var subTables = GetExcelTable();

        var tableTotals = subTables.Select(_ => _.TotalRow);
        decimal accordato = tableTotals.Select(_ => _.Row?.Accordato ?? 0).Sum();
        decimal sbilancio = tableTotals.Select(_ => _.Row?.Sbilancio ?? 0).Sum();

        int totalCount = subTables.Select(_ => _.Rows?.Count ?? 0).Sum();

        var mainTotal = new TotalRow<AgreedOtherThanUsedForPooledTransactionsItem>(
            new AgreedOtherThanUsedForPooledTransactionsItem
            {
                Accordato = accordato,
                Sbilancio = sbilancio,
                Utilizzato = accordato - sbilancio
            },
            $"Totale cubo ALL OF POSSIBLE per n° {totalCount} Forme Tecniche"
        );

        var table = new AgreedOtherThanUsedForPooledTransactionsExcelReportTable(subTables, mainTotal);

        builder.AddCompanyLine(companyLine);
        builder.AddFilterSection(filterSection);
        builder.AddTable(table);
    }
}