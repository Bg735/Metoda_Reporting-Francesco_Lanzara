using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Metoda.Reporting.Models.Reports.SufferingsReportedWithOtherPhenomena;

public static class SufferingsReportedWithOtherPhenomenaFakeData
{
    public static string[] _centsitos = new[] { "12345 - Soggetto A", "45687 - Soggetto A", "77295 - Soggetto B" };

    private static void FillSubtables(
        out List<SufferingsReportedWithOtherPhenomenaItem> subTable, 
        out TotalRow<SufferingsReportedWithOtherPhenomenaItem> totalRow)
    {
        Random random = new();
        decimal sofferenza, altroCubo, totale;

        subTable = new List<SufferingsReportedWithOtherPhenomenaItem>();

        for (int i = 0; i < 50; i++)
        {
            for (int k = 0; k < _centsitos.Length; k++)
            {
                sofferenza = random.Next(10_000, 15_000);
                altroCubo = random.Next(1000, 2000);
                totale = random.Next(1000, 2500);

                subTable.Add(new SufferingsReportedWithOtherPhenomenaItem
                {
                    CodCensito = _centsitos[k],
                    StatoRapporto = "138 - Altri Crediti",
                    Sofferenza = sofferenza,
                    Cubo2 = "550400 - Rischi a scadenza",
                    AltroCubo = altroCubo,
                    Totale = totale
                });
            }
        }

        sofferenza = subTable.Select(_ => _.Sofferenza).Sum();
        altroCubo = subTable.Select(_ => _.AltroCubo).Sum();
        totale = subTable.Select(_ => _.Totale).Sum();

        totalRow = new TotalRow<SufferingsReportedWithOtherPhenomenaItem>(
            new SufferingsReportedWithOtherPhenomenaItem
            {
                Sofferenza = sofferenza,
                AltroCubo = altroCubo,
                Totale = totale
            });
    }

    public static IList<ExcelTable<SufferingsReportedWithOtherPhenomenaItem>> GetExcelTable()
    {
        var list = new List<ExcelTable<SufferingsReportedWithOtherPhenomenaItem>>();
        List<SufferingsReportedWithOtherPhenomenaItem> subTable;
        TotalRow<SufferingsReportedWithOtherPhenomenaItem> totalRow;

        FillSubtables(out subTable, out totalRow);

        var res = new ExcelTable<SufferingsReportedWithOtherPhenomenaItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static IList<PdfTable<SufferingsReportedWithOtherPhenomenaItem>> GetPdfTable()
    {
        var list = new List<PdfTable<SufferingsReportedWithOtherPhenomenaItem>>();
        List<SufferingsReportedWithOtherPhenomenaItem> subTable;
        TotalRow<SufferingsReportedWithOtherPhenomenaItem> totalRow;

        FillSubtables(out subTable, out totalRow);

        var res = new PdfTable<SufferingsReportedWithOtherPhenomenaItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static void FillBuilderByData(SufferingsReportedWithOtherPhenomenaPdfReportBuilder builder)
    {
        var companyLine = new PdfReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var subTables = GetPdfTable();

        var table = new SufferingsReportedWithOtherPhenomenaPdfReportTable(subTables, null);

        builder.AddCompanyLine(companyLine);
        builder.AddTable(table);
    }

    public static void FillBuilderByData(SufferingsReportedWithOtherPhenomenaExcelReportBuilder builder)
    {
        var companyLine = new ExcelReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var filterSection = new ExcelReportFilterSection("Filter",
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Labore molestiae ipsam nemo iure! Recusandae nulla, fugiat ad voluptatibus impedit similique laboriosam tenetur alias! Sunt magni porro veritatis quos, laborum fugiat.");


        var subTables = GetExcelTable();

        var table = new SufferingsReportedWithOtherPhenomenaExcelReportTable(subTables, null);


        builder.AddCompanyLine(companyLine);
        builder.AddFilterSection(filterSection);
        builder.AddTable(table);
    }
}