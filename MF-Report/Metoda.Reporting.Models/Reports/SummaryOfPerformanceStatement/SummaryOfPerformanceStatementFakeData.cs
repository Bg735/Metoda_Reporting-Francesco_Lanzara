using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Metoda.Reporting.Models.Reports.SummaryOfPerformanceStatement;

public static class SummaryOfPerformanceStatementFakeData
{
    public static string[] _totals = new[] {
                "Totale Crediti per Cassa",
                "Totale Crediti di Firma",
                "Totale Garanzie Ricevute",
                "Totale Derivati Finanziari",
                "Totale Sezione Informativa"
            };

    public static string[][] _cubies = new[]{
                new []{
                    "550200 - Rischi Autoliquidanti",
                    "550400 - Rischi a Scadenza",
                    "550600 - Rischi a Revoca",
                    "550800 - Finanziamenti a Procedura Concorsuale e altri Finanziamenti Particolari",
                    "551000 - Sofferenze"
                },
                new []{
                    "552200 - Garanzie Connesse con Operazioni di Natura Commerciale",
                    "552400 - Garanzie Connesse con Operazioni di Natura Finanziaria"
                },
                new []{
                    "553200 - Garanzie Ricevute"
                },
                new []{
                    "553300 - Derivati Finanziari"
                },
                new []{
                    "554800 - Operazioni effettuate per conto di terzi",
                    "554900 - Crediti per cassa: operazioni in “pool” -azienda capofila",
                    "554901 - Crediti per cassa: operazioni in “pool” -altra azienda partecipante",
                    "554902 - Crediti per cassa: operazioni in “pool” – totale",
                    "554800 - Operazioni effettuate per conto di terzi",
                    "555100 - Crediti acquisiti(originariamente) da clientela diversa da intermediari - debitori ceduti",
                    "555150 - Rischi autoliquidanti - crediti scaduti",
                    "555200 - Sofferenze - crediti passati a perdita",
                    "555400 - Sofferenze - crediti ceduti a terzi"
                }
            };

    private static void FillSubtables(out List<SummaryOfPerformanceStatementItem> subTable, out TotalRow<SummaryOfPerformanceStatementItem> totalRow, int counter)
    {
        Random random = new();

        subTable = new List<SummaryOfPerformanceStatementItem>();

        foreach (var c in _cubies[counter])
            subTable.Add(new SummaryOfPerformanceStatementItem
            {
                Cubi = c,
                NumSegnMese = random.Next(1000, 18000),
                TotaleMese = random.Next(1000, 10000),
                NumSegnMesePrec = random.Next(1, 18),
                TotaleMesePrec = random.Next(1000, 10000),
            });

        totalRow = new TotalRow<SummaryOfPerformanceStatementItem>(
            new SummaryOfPerformanceStatementItem
            {
                NumSegnMese = subTable.Select(_ => _.NumSegnMese).Sum(),
                NumSegnMesePrec = subTable.Select(_ => _.NumSegnMesePrec).Sum(),
                TotaleMese = subTable.Select(_ => _.TotaleMese).Sum(),
                TotaleMesePrec = subTable.Select(_ => _.TotaleMesePrec).Sum()
            }, _totals[counter]);
    }

    public static IList<ExcelTable<SummaryOfPerformanceStatementItem>> GetExcelTable()
    {
        var list = new List<ExcelTable<SummaryOfPerformanceStatementItem>>();

        for (int k = 0; k < _totals.Length; k++)
        {
            FillSubtables(out List<SummaryOfPerformanceStatementItem> subTable, out TotalRow<SummaryOfPerformanceStatementItem> totalRow, k);

            var res = new ExcelTable<SummaryOfPerformanceStatementItem>(subTable, totalRow);

            list.Add(res);
        }
        return list;
    }

    public static IList<PdfTable<SummaryOfPerformanceStatementItem>> GetPdfTable()
    {
        var list = new List<PdfTable<SummaryOfPerformanceStatementItem>>();

        for (int k = 0; k < _totals.Length; k++)
        {
            FillSubtables(out List<SummaryOfPerformanceStatementItem> subTable, out TotalRow<SummaryOfPerformanceStatementItem> totalRow, k);

            var res = new PdfTable<SummaryOfPerformanceStatementItem>(subTable, totalRow);

            list.Add(res);
        }
        return list;
    }

    public static void FillBuilderByData(SummaryOfPerformanceStatementPdfReportBuilder builder)
    {
        var companyLine = new PdfReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var subTables = GetPdfTable();

        var table = new SummaryOfPerformanceStatementPdfReportTable(subTables, null);

        builder.AddCompanyLine(companyLine);
        builder.AddTable(table);
    }

    public static void FillBuilderByData(SummaryOfPerformanceStatementExcelReportBuilder builder)
    {
        var companyLine = new ExcelReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var filterSection = new ExcelReportFilterSection("Filter",
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Labore molestiae ipsam nemo iure! Recusandae nulla, fugiat ad voluptatibus impedit similique laboriosam tenetur alias! Sunt magni porro veritatis quos, laborum fugiat.");


        var subTables = GetExcelTable();

        var table = new SummaryOfPerformanceStatementExcelReportTable(subTables, null);


        builder.AddCompanyLine(companyLine);
        builder.AddFilterSection(filterSection);
        builder.AddTable(table);
    }
}