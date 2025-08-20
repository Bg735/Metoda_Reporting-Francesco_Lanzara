using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Metoda.Reporting.Models.Reports.PresenceOfLeadPoolAndNotTotalPoolOrViceVersa;

public static class PresenceOfLeadPoolAndNotTotalPoolOrViceVersaFakeData
{
    public static string[] _centsitos = new[] { "12345 - Soggetto A", "45687 - Soggetto A", "77295 - Soggetto B" };

    private static void FillSubtables(
        out List<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem> subTable,
        out TotalRow<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem> totalRow,
        int counter)
    {
        Random random = new();
        decimal capAccordato, capUtilizzato, totAccordato, totUtilizzato;

        subTable = new List<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem>();

        for (int i = 0; i < 7; i++)
        {
            capAccordato = random.Next(4000, 10000);
            capUtilizzato = random.Next(1000, 4000);
            totAccordato = capAccordato - random.Next(300, 500);
            totUtilizzato = capUtilizzato - random.Next(100, 400);

            subTable.Add(new PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem
            {
                CodCensito = _centsitos[counter],
                Cubo = $"{random.Next(554900, 554902)} - Sezione informativa – crediti per cassa: operazioni in \"pool\"",
                CapofilaUtilizzato = capAccordato,
                CapofilaAccordato = capUtilizzato,
                TotAccordato = totAccordato,
                TotUtilizzato = totUtilizzato
            });
        }

        totalRow = new TotalRow<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem>(
        new PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem
        {
            CapofilaUtilizzato = subTable.Select(_ => _.CapofilaAccordato).Sum(),
            CapofilaAccordato = subTable.Select(_ => _.CapofilaUtilizzato).Sum(),
            TotAccordato = subTable.Select(_ => _.TotAccordato).Sum(),
            TotUtilizzato = subTable.Select(_ => _.TotUtilizzato).Sum()
        });
    }

    public static IList<ExcelTable<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem>> GetExcelTable()
    {
        var list = new List<ExcelTable<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem>>();
        List<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem> subTable;
        TotalRow<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem> totalRow;


        for (int k = 0; k < _centsitos.Length; k++)
        {
            FillSubtables(out subTable, out totalRow, k);

            var res = new ExcelTable<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem>(subTable, totalRow);

            list.Add(res);
        }
        return list;
    }

    public static IList<PdfTable<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem>> GetPdfTable()
    {
        var list = new List<PdfTable<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem>>();
        List<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem> subTable;
        TotalRow<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem> totalRow;

        for (int k = 0; k < _centsitos.Length; k++)
        {
            FillSubtables(out subTable, out totalRow, k);

            var res = new PdfTable<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem>(subTable, totalRow);

            list.Add(res);
        }

        return list;
    }

    public static void FillBuilderByData(PresenceOfLeadPoolAndNotTotalPoolOrViceVersaPdfReportBuilder builder)
    {
        var companyLine = new PdfReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var subTables = GetPdfTable();
        var tableTotals = subTables.Select(_ => _.TotalRow);

        var mainTotal = new TotalRow<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem>(
         new PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem
         {
             CapofilaUtilizzato = tableTotals.Select(_ => _.Row.CapofilaAccordato).Sum(),
             CapofilaAccordato = tableTotals.Select(_ => _.Row.CapofilaUtilizzato).Sum(),
             TotAccordato = tableTotals.Select(_ => _.Row.TotAccordato).Sum(),
             TotUtilizzato = tableTotals.Select(_ => _.Row.TotUtilizzato).Sum()
         });

        var table = new PresenceOfLeadPoolAndNotTotalPoolOrViceVersaPdfReportTable(subTables, mainTotal);

        builder.AddCompanyLine(companyLine);
        builder.AddTable(table);
    }

    public static void FillBuilderByData(PresenceOfLeadPoolAndNotTotalPoolOrViceVersaExcelReportBuilder builder)
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

        var mainTotal = new TotalRow<PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem>(
         new PresenceOfLeadPoolAndNotTotalPoolOrViceVersaItem
         {
             CapofilaUtilizzato = tableTotals.Select(_ => _.Row.CapofilaAccordato).Sum(),
             CapofilaAccordato = tableTotals.Select(_ => _.Row.CapofilaUtilizzato).Sum(),
             TotAccordato = tableTotals.Select(_ => _.Row.TotAccordato).Sum(),
             TotUtilizzato = tableTotals.Select(_ => _.Row.TotUtilizzato).Sum()
         });

        var table = new PresenceOfLeadPoolAndNotTotalPoolOrViceVersaExcelReportTable(subTables, mainTotal);

        builder.AddCompanyLine(companyLine);
        builder.AddFilterSection(filterSection);
        builder.AddTable(table);
    }
}