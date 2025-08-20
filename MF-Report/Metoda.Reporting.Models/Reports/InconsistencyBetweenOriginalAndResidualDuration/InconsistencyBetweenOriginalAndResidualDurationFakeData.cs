using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Models.Reports.InconsistencyBetweenOriginalAndResidualDuration;

public static class InconsistencyBetweenOriginalAndResidualDurationFakeData
{
    private static readonly string[] _centsitos = new[] { "12345 - Soggetto A", "45687 - Soggetto A", "77295 - Soggetto B" };

    private static void FillSubtables(
        out List<InconsistencyBetweenOriginalAndResidualDurationItem> subTable, 
        out TotalRow<InconsistencyBetweenOriginalAndResidualDurationItem> totalRow,
        int counter)
    {
        Random random = new();
        decimal accordato, utilizzato;

        subTable = new List<InconsistencyBetweenOriginalAndResidualDurationItem>();

        for (int i = 0; i < 2; i++)
        {
            accordato = random.Next(4000, 10000);
            utilizzato = random.Next(1000, 4000);
            subTable.Add(new InconsistencyBetweenOriginalAndResidualDurationItem
            {
                Accordato = accordato,
                CodCensito = _centsitos[counter],
                Cubo = $"{random.Next(555100, 555202)} - Sezione Informativa",
                DurataOriginaria = "18 – Oltre 1 anno",
                DurataResidua = "5 – fino ad un anno",
                Utilizzato = utilizzato,
                Sbilancio = accordato - utilizzato
            });
        }

        accordato = subTable.Select(_ => _.Accordato).Sum();
        utilizzato = subTable.Select(_ => _.Utilizzato).Sum();

        totalRow = new TotalRow<InconsistencyBetweenOriginalAndResidualDurationItem>(
            new InconsistencyBetweenOriginalAndResidualDurationItem
            {
                Accordato = accordato,
                Utilizzato = utilizzato,
                Sbilancio = accordato - utilizzato
            });
    }

    public static IList<ExcelTable<InconsistencyBetweenOriginalAndResidualDurationItem>> GetExcelTable()
    {
        var list = new List<ExcelTable<InconsistencyBetweenOriginalAndResidualDurationItem>>();

        for (int k = 0; k < _centsitos.Length; k++)
        {
            FillSubtables(out List<InconsistencyBetweenOriginalAndResidualDurationItem> subTable, out TotalRow<InconsistencyBetweenOriginalAndResidualDurationItem> totalRow, k);

            var res = new ExcelTable<InconsistencyBetweenOriginalAndResidualDurationItem>(subTable, totalRow);

            list.Add(res);
        }

        return list;
    }

    public static IList<PdfTable<InconsistencyBetweenOriginalAndResidualDurationItem>> GetPdfTable()
    {
        var list = new List<PdfTable<InconsistencyBetweenOriginalAndResidualDurationItem>>();

        for (int k = 0; k < _centsitos.Length; k++)
        {
            FillSubtables(out List<InconsistencyBetweenOriginalAndResidualDurationItem> subTable, out TotalRow<InconsistencyBetweenOriginalAndResidualDurationItem> totalRow, k);

            var res = new PdfTable<InconsistencyBetweenOriginalAndResidualDurationItem>(subTable, totalRow);

            list.Add(res);
        }

        return list;
    }

    public static void FillBuilderByData(InconsistencyBetweenOriginalAndResidualDurationPdfReportBuilder builder)
    {
        var companyLine = new PdfReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var subTables = GetPdfTable();

        var tableTotals = subTables.Select(_ => _.TotalRow);
        decimal accordato = tableTotals.Select(_ => _.Row?.Accordato ?? 0).Sum();
        decimal sbilancio = tableTotals.Select(_ => _.Row?.Sbilancio ?? 0).Sum();


        var mainTotal = new TotalRow<InconsistencyBetweenOriginalAndResidualDurationItem>(
         new InconsistencyBetweenOriginalAndResidualDurationItem
         {
             Accordato = accordato,
             Sbilancio = sbilancio,
             Utilizzato = accordato - sbilancio
         });

        var table = new InconsistencyBetweenOriginalAndResidualDurationPdfReportTable(subTables, mainTotal);

        builder.AddCompanyLine(companyLine);
        builder.AddTable(table);
    }

    public static void FillBuilderByData(InconsistencyBetweenOriginalAndResidualDurationExcelReportBuilder builder)
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


        var mainTotal = new TotalRow<InconsistencyBetweenOriginalAndResidualDurationItem>(
         new InconsistencyBetweenOriginalAndResidualDurationItem
         {
             Accordato = accordato,
             Sbilancio = sbilancio,
             Utilizzato = accordato - sbilancio
         });

        var table = new InconsistencyBetweenOriginalAndResidualDurationExcelReportTable(subTables, mainTotal);

        builder.AddCompanyLine(companyLine);
        builder.AddFilterSection(filterSection);
        builder.AddTable(table);
    }
}