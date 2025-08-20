using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Metoda.Reporting.Models.Reports.Trespassing;

public static class TrespassingFakeData
{
    private static void FillSubtables(out List<TrespassingItem> subTable, out TotalRow<TrespassingItem> totalRow)
    {
        var centsitos = new[] { "12345 - Soggetto A", "45687 - Soggetto A", "77295 - Soggetto B" };

        Random random = new();
        decimal accordato, utilizzato;

        subTable = new List<TrespassingItem>();

        for (int k = 0; k < centsitos.Length; k++)
        {
            accordato = random.Next(4000, 10000);
            utilizzato = random.Next(1000, 4000);
            subTable.Add(new TrespassingItem
            {
                Accordato = accordato,
                CodCensito = centsitos[k],
                Cubo = "550200 - Crediti per cassa - Rischi Autoliquidanti",
                Utilizzato = utilizzato,
                Sbilancio = accordato - utilizzato
            });
        }

        accordato = subTable.Select(_ => _.Accordato).Sum();
        utilizzato = subTable.Select(_ => _.Utilizzato).Sum();

        totalRow = new TotalRow<TrespassingItem>(
                new TrespassingItem
                {
                    Accordato = accordato,
                    Utilizzato = utilizzato,
                    Sbilancio = accordato - utilizzato
                });

    }

    public static IList<ExcelTable<TrespassingItem>> GetExcelTable()
    {
        var list = new List<ExcelTable<TrespassingItem>>();

        FillSubtables(out List<TrespassingItem> subTable, out TotalRow<TrespassingItem> totalRow);

        var res = new ExcelTable<TrespassingItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static IList<PdfTable<TrespassingItem>> GetPdfTable()
    {
        var list = new List<PdfTable<TrespassingItem>>();

        FillSubtables(out List<TrespassingItem> subTable, out TotalRow<TrespassingItem> totalRow);

        var res = new PdfTable<TrespassingItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static void FillBuilderByData(TrespassingPdfReportBuilder builder)
    {
        var companyLine = new PdfReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var subTables = GetPdfTable();

        var table = new TrespassingPdfReportTable(subTables, null);

        builder.AddCompanyLine(companyLine);
        builder.AddTable(table);
    }

    public static void FillBuilderByData(TrespassingExcelReportBuilder builder)
    {
        var companyLine = new ExcelReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var filterSection = new ExcelReportFilterSection("Filter",
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Labore molestiae ipsam nemo iure! Recusandae nulla, fugiat ad voluptatibus impedit similique laboriosam tenetur alias! Sunt magni porro veritatis quos, laborum fugiat.");


        var subTables = GetExcelTable();

        var table = new TrespassingExcelReportTable(subTables, null);


        builder.AddCompanyLine(companyLine);
        builder.AddFilterSection(filterSection);
        builder.AddTable(table);
    }
}