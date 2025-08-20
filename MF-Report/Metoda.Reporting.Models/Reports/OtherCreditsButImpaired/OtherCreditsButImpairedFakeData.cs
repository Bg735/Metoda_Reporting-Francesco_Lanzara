using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Metoda.Reporting.Models.Reports.OtherCreditsButImpaired;

public static class OtherCreditsButImpairedFakeData
{
    private static void FillSubtables(out List<OtherCreditsButImpairedItem> subTable, out TotalRow<OtherCreditsButImpairedItem> totalRow)
    {
        var centsitos = new[] { "12345 - Soggetto A", "45687 - Soggetto A", "77295 - Soggetto B" };

        Random random = new();
        decimal utilizzato;

        subTable = new List<OtherCreditsButImpairedItem>();

        for (int i = 0; i < 1500; i++)
            for (int k = 0; k < centsitos.Length; k++)
            {
                utilizzato = random.Next(1000, 4000);
                subTable.Add(new OtherCreditsButImpairedItem
                {
                    CodCensito = centsitos[k],
                    Cubo = "550200 - Crediti per cassa - Rischi Autoliquidanti",
                    StatoRapporto = "138 - Altri Crediti",
                    QualitaCredito = "1 - Deteriorato",
                    Utilizzato = utilizzato
                });
            }

        totalRow = new TotalRow<OtherCreditsButImpairedItem>(
            new OtherCreditsButImpairedItem
            {
                Utilizzato = subTable.Select(_ => _.Utilizzato).Sum()
            }, "Totale");
    }

    public static IList<ExcelTable<OtherCreditsButImpairedItem>> GetExcelTable()
    {
        var list = new List<ExcelTable<OtherCreditsButImpairedItem>>();

        FillSubtables(out List<OtherCreditsButImpairedItem> subTable, out TotalRow<OtherCreditsButImpairedItem> totalRow);

        var res = new ExcelTable<OtherCreditsButImpairedItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static IList<PdfTable<OtherCreditsButImpairedItem>> GetPdfTable()
    {
        var list = new List<PdfTable<OtherCreditsButImpairedItem>>();

        FillSubtables(out List<OtherCreditsButImpairedItem> subTable, out TotalRow<OtherCreditsButImpairedItem> totalRow);

        var res = new PdfTable<OtherCreditsButImpairedItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static void FillBuilderByData(OtherCreditsButImpairedPdfReportBuilder builder)
    {
        var companyLine = new PdfReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var subTables = GetPdfTable();

        var table = new OtherCreditsButImpairedPdfReportTable(subTables, null);

        builder.AddCompanyLine(companyLine);
        builder.AddTable(table);
    }

    public static void FillBuilderByData(OtherCreditsButImpairedExcelReportBuilder builder)
    {
        var companyLine = new ExcelReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var filterSection = new ExcelReportFilterSection("Filter",
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Labore molestiae ipsam nemo iure! Recusandae nulla, fugiat ad voluptatibus impedit similique laboriosam tenetur alias! Sunt magni porro veritatis quos, laborum fugiat.");


        var subTables = GetExcelTable();

        var table = new OtherCreditsButImpairedExcelReportTable(subTables, null);


        builder.AddCompanyLine(companyLine);
        builder.AddFilterSection(filterSection);
        builder.AddTable(table);
    }
}