using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Models.Reports.AbsenceRegisteredConnected;

public static class AbsenceRegisteredConnectedFakeData
{
    private static void FillSubtables(out List<AbsenceRegisteredConnectedItem> subTable, out TotalRow<AbsenceRegisteredConnectedItem> totalRow)
    {
        var centsitos = new[] { "12345 - Soggetto A", "45687 - Soggetto A", "77295 - Soggetto B" };

        Random random = new();
        decimal valore;

        subTable = new List<AbsenceRegisteredConnectedItem>();

        for (int k = 0; k < centsitos.Length; k++)
        {
            valore = random.Next(1000, 4000);
            subTable.Add(new AbsenceRegisteredConnectedItem
            {
                Valore = valore,
                CodCensito = centsitos[k],
                Fenomeno = "555100 - Crediti per cassa - Rischi Autoliquidanti",
                StatoRapporto = "138 - Altri Crediti",
            });
        }

        totalRow = new TotalRow<AbsenceRegisteredConnectedItem>(
            new AbsenceRegisteredConnectedItem
            {
                Valore = subTable.Select(_ => _.Valore).Sum()
            }, "Totale");
    }

    public static IList<ExcelTable<AbsenceRegisteredConnectedItem>> GetExcelTable()
    {
        var list = new List<ExcelTable<AbsenceRegisteredConnectedItem>>();
        for (int i = 0; i < 3; i++)
        {

            FillSubtables(out List<AbsenceRegisteredConnectedItem> subTable, out TotalRow<AbsenceRegisteredConnectedItem> totalRow);

            var res = new ExcelTable<AbsenceRegisteredConnectedItem>(subTable, totalRow);

            list.Add(res);
        }
        return list;
    }

    public static IList<PdfTable<AbsenceRegisteredConnectedItem>> GetPdfTable()
    {
        var list = new List<PdfTable<AbsenceRegisteredConnectedItem>>();

        for (int i = 0; i < 3; i++)
        {
            FillSubtables(out List<AbsenceRegisteredConnectedItem> subTable, out TotalRow<AbsenceRegisteredConnectedItem> totalRow);

            var res = new PdfTable<AbsenceRegisteredConnectedItem>(subTable, totalRow);

            list.Add(res);
        }

        return list;
    }

    public static void FillBuilderByData(AbsenceRegisteredConnectedPdfReportBuilder builder)
    {
        var companyLine = new PdfReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var filterSection = new PdfReportFilterSection("Filter", "Cubo = 50200 AND Utilizzato > 10.000");

        var subTables = GetPdfTable();
        var tableTotals = subTables.Select(_ => _.TotalRow);
        decimal valore = tableTotals.Select(_ => _.Row?.Valore ?? 0).Sum();

        var mainTotal = new TotalRow<AbsenceRegisteredConnectedItem>(
            new AbsenceRegisteredConnectedItem
            {
                Valore = valore
            }, "Main Total"
         );

        var table0 = new AbsenceRegisteredConnectedPdfReportTable(subTables, mainTotal);
        var table1 = new AbsenceRegisteredConnectedPdfReportTable(subTables, mainTotal);
        var table2 = new AbsenceRegisteredConnectedPdfReportTable(subTables, null);
        var table3 = new AbsenceRegisteredConnectedPdfReportTable(subTables, null);


        builder.AddCompanyLine(companyLine);
        builder.AddFilterSection(filterSection);
        builder.AddTable(table0);
        builder.AddTable(table1, Common.Enums.IntermediateTotalLocation.TableBody);
        builder.AddTable(table2);
        builder.AddTable(table3, Common.Enums.IntermediateTotalLocation.TableBody);
    }

    public static void FillBuilderByData(AbsenceRegisteredConnectedExcelReportBuilder builder)
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
        decimal valore = tableTotals.Select(_ => _.Row?.Valore ?? 0).Sum();

        var mainTotal = new TotalRow<AbsenceRegisteredConnectedItem>(
           new AbsenceRegisteredConnectedItem
           {
               Valore = valore
           }, "Main Total"
        );

        var table0 = new AbsenceRegisteredConnectedExcelReportTable(subTables, mainTotal);
        var table1 = new AbsenceRegisteredConnectedExcelReportTable(subTables, mainTotal);
        var table2 = new AbsenceRegisteredConnectedExcelReportTable(subTables, null);
        var table3 = new AbsenceRegisteredConnectedExcelReportTable(subTables, null);


        builder.AddCompanyLine(companyLine);
        builder.AddFilterSection(filterSection);
        builder.AddTable(table0);
        builder.AddTable(table1, Common.Enums.IntermediateTotalLocation.TableBody);
        builder.AddTable(table2);
        builder.AddTable(table3, Common.Enums.IntermediateTotalLocation.TableBody);

    }
}