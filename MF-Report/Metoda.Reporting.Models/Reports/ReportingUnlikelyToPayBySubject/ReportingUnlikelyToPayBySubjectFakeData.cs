using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Metoda.Reporting.Models.Reports.ReportingUnlikelyToPayBySubject;

public static class ReportingUnlikelyToPayBySubjectFakeData
{
    private static void FillSubtables(
        out List<ReportingUnlikelyToPayBySubjectItem> subTable, 
        out TotalRow<ReportingUnlikelyToPayBySubjectItem> totalRow)
    {
        var centsitos = new[] { "12345 - Soggetto A", "45687 - Soggetto A", "77295 - Soggetto B" };

        Random random = new();
        decimal adIncaglio, nonIncaglio;

        subTable = new List<ReportingUnlikelyToPayBySubjectItem>();

        for (int k = 0; k < centsitos.Length; k++)
        {
            adIncaglio = random.Next(4000, 10000);
            nonIncaglio = random.Next(1000, 4000);
            subTable.Add(new ReportingUnlikelyToPayBySubjectItem
            {
                StatoRapporto = "134 - <Descrizione>",
                CodCensito = centsitos[k],
                Cubo = $"{random.Next(554900, 554902)} - Sezione Informativa – Crediti per Cassa: Operazioni in pool – Azienda capofila",
                AdIncaglio = adIncaglio,
                NonIncaglio = nonIncaglio
            });
        }

        adIncaglio = subTable.Select(_ => _.AdIncaglio).Sum();
        nonIncaglio = subTable.Select(_ => _.NonIncaglio).Sum();

        totalRow = new TotalRow<ReportingUnlikelyToPayBySubjectItem>(
            new ReportingUnlikelyToPayBySubjectItem
            {
                AdIncaglio = adIncaglio,
                NonIncaglio = nonIncaglio
            });
    }

    public static IList<ExcelTable<ReportingUnlikelyToPayBySubjectItem>> GetExcelTable()
    {
        var list = new List<ExcelTable<ReportingUnlikelyToPayBySubjectItem>>();

        FillSubtables(out List<ReportingUnlikelyToPayBySubjectItem> subTable, out TotalRow<ReportingUnlikelyToPayBySubjectItem> totalRow);

        var res = new ExcelTable<ReportingUnlikelyToPayBySubjectItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static IList<PdfTable<ReportingUnlikelyToPayBySubjectItem>> GetPdfTable()
    {
        var list = new List<PdfTable<ReportingUnlikelyToPayBySubjectItem>>();

        FillSubtables(out List<ReportingUnlikelyToPayBySubjectItem> subTable, out TotalRow<ReportingUnlikelyToPayBySubjectItem> totalRow);

        var res = new PdfTable<ReportingUnlikelyToPayBySubjectItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static void FillBuilderByData(ReportingUnlikelyToPayBySubjectPdfReportBuilder builder)
    {
        var companyLine = new PdfReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var subTables = GetPdfTable();

        var table = new ReportingUnlikelyToPayBySubjectPdfReportTable(subTables, null);

        builder.AddCompanyLine(companyLine);
        builder.AddTable(table);
    }

    public static void FillBuilderByData(ReportingUnlikelyToPayBySubjectExcelReportBuilder builder)
    {
        var companyLine = new ExcelReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var filterSection = new ExcelReportFilterSection("Filter",
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Labore molestiae ipsam nemo iure! Recusandae nulla, fugiat ad voluptatibus impedit similique laboriosam tenetur alias! Sunt magni porro veritatis quos, laborum fugiat.");


        var subTables = GetExcelTable();

        var table = new ReportingUnlikelyToPayBySubjectExcelReportTable(subTables, null);


        builder.AddCompanyLine(companyLine);
        builder.AddFilterSection(filterSection);
        builder.AddTable(table);
    }
}