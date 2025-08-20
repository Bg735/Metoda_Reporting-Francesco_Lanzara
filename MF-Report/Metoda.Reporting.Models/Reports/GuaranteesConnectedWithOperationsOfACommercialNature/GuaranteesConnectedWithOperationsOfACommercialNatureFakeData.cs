using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Metoda.Reporting.Models.Reports.GuaranteesConnectedWithOperationsOfACommercialNature;

public static class GuaranteesConnectedWithOperationsOfACommercialNatureFakeData
{
    private static void FillSubtables(out List<GuaranteesConnectedWithOperationsOfACommercialNatureItem> subTable, out TotalRow<GuaranteesConnectedWithOperationsOfACommercialNatureItem> totalRow)
    {
        var centsito = "12345 - Soggetto A";
        Random random = new();
        decimal utilizzato;

        subTable = new List<GuaranteesConnectedWithOperationsOfACommercialNatureItem>();

        for (int k = 0; k < 10; k++)
        {
            utilizzato = random.Next(1000, 4000);
            subTable.Add(new GuaranteesConnectedWithOperationsOfACommercialNatureItem
            {
                Utilizzato = utilizzato,
                CodCensito = centsito,
                Localizzazione = "00155 - <descrizione>",
                ImportOrExport = "1 - <descrizione>",
                Divisa = "1 - Euro",
                StatoRapporto = "134 - <descrizione>"
            });
        }

        totalRow = new TotalRow<GuaranteesConnectedWithOperationsOfACommercialNatureItem>(
            new GuaranteesConnectedWithOperationsOfACommercialNatureItem
            {
                Utilizzato = subTable.Select(_ => _.Utilizzato).Sum()
            },
            "Custom Total Label");
    }

    public static IList<ExcelTable<GuaranteesConnectedWithOperationsOfACommercialNatureItem>> GetExcelTable()
    {
        var list = new List<ExcelTable<GuaranteesConnectedWithOperationsOfACommercialNatureItem>>();

        FillSubtables(out List<GuaranteesConnectedWithOperationsOfACommercialNatureItem> subTable, out TotalRow<GuaranteesConnectedWithOperationsOfACommercialNatureItem> totalRow);

        var res = new ExcelTable<GuaranteesConnectedWithOperationsOfACommercialNatureItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static IList<PdfTable<GuaranteesConnectedWithOperationsOfACommercialNatureItem>> GetPdfTable()
    {
        var list = new List<PdfTable<GuaranteesConnectedWithOperationsOfACommercialNatureItem>>();

        FillSubtables(out List<GuaranteesConnectedWithOperationsOfACommercialNatureItem> subTable, out TotalRow<GuaranteesConnectedWithOperationsOfACommercialNatureItem> totalRow);

        var res = new PdfTable<GuaranteesConnectedWithOperationsOfACommercialNatureItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static void FillBuilderByData(GuaranteesConnectedWithOperationsOfACommercialNaturePdfReportBuilder builder)
    {
        var companyLine = new PdfReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var subTables = GetPdfTable();

        var tableTotals = subTables.Select(_ => _.TotalRow);

        var mainTotal = new TotalRow<GuaranteesConnectedWithOperationsOfACommercialNatureItem>(
            new GuaranteesConnectedWithOperationsOfACommercialNatureItem
            {
                Utilizzato = tableTotals.Select(_ => _.Row?.Utilizzato ?? 0).Sum()
            });

        var table = new GuaranteesConnectedWithOperationsOfACommercialNaturePdfReportTable(subTables, mainTotal);

        builder.AddCompanyLine(companyLine);
        builder.AddTable(table);
    }

    public static void FillBuilderByData(GuaranteesConnectedWithOperationsOfACommercialNatureExcelReportBuilder builder)
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

        var mainTotal = new TotalRow<GuaranteesConnectedWithOperationsOfACommercialNatureItem>(
            new GuaranteesConnectedWithOperationsOfACommercialNatureItem
            {
                Utilizzato = tableTotals.Select(_ => _.Row?.Utilizzato ?? 0).Sum()
            });

        var table = new GuaranteesConnectedWithOperationsOfACommercialNatureExcelReportTable(subTables, mainTotal);


        builder.AddCompanyLine(companyLine);
        builder.AddFilterSection(filterSection);
        builder.AddTable(table);
    }
}