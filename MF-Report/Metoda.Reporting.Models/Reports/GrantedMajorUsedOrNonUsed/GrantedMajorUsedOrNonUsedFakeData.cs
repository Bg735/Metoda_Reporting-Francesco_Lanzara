using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Models.Reports.GrantedMajorUsedOrNonUsed;

public static class GrantedMajorUsedOrNonUsedFakeData
{
    private static readonly string[] _centsitos = new[] { "12345 - Soggetto A", "45687 - Soggetto A", "77295 - Soggetto B" };

    private static void FillSubtables(out List<GrantedMajorUsedOrNonUsedItem> subTable, out TotalRow<GrantedMajorUsedOrNonUsedItem> totalRow)
    {
        Random random = new();
        decimal accordato, utilizzato;

        subTable = new List<GrantedMajorUsedOrNonUsedItem>();

        for (int k = 0; k < _centsitos.Length; k++)
        {
            accordato = random.Next(4000, 10000);
            utilizzato = random.Next(1000, 4000);
            subTable.Add(new GrantedMajorUsedOrNonUsedItem
            {
                Accordato = accordato,
                CodCensito = _centsitos[k],
                NDG = "New NDG 550200",
                Cubo = "550200 - Crediti per cassa - Rischi Autoliquidanti",
                Utilizzato = utilizzato,
                Sbilancio = accordato - utilizzato
            });
        }

        accordato = subTable.Select(_ => _.Accordato).Sum();
        utilizzato = subTable.Select(_ => _.Utilizzato).Sum();

        totalRow = new TotalRow<GrantedMajorUsedOrNonUsedItem>(
            new GrantedMajorUsedOrNonUsedItem
            {
                Accordato = accordato,
                Utilizzato = utilizzato,
                Sbilancio = accordato - utilizzato
            });
    }

    public static IList<ExcelTable<GrantedMajorUsedOrNonUsedItem>> GetExcelTable()
    {
        var list = new List<ExcelTable<GrantedMajorUsedOrNonUsedItem>>();

        FillSubtables(out List<GrantedMajorUsedOrNonUsedItem> subTable, out TotalRow<GrantedMajorUsedOrNonUsedItem> totalRow);

        var res = new ExcelTable<GrantedMajorUsedOrNonUsedItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static IList<PdfTable<GrantedMajorUsedOrNonUsedItem>> GetPdfTable()
    {
        var list = new List<PdfTable<GrantedMajorUsedOrNonUsedItem>>();

        FillSubtables(out List<GrantedMajorUsedOrNonUsedItem> subTable, out TotalRow<GrantedMajorUsedOrNonUsedItem> totalRow);

        var res = new PdfTable<GrantedMajorUsedOrNonUsedItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static void FillBuilderByData(GrantedMajorUsedOrNonUsedPdfReportBuilder builder)
    {
        var companyLine = new PdfReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var subTables = GetPdfTable();

        var table = new GrantedMajorUsedOrNonUsedPdfReportTable(subTables, null);

        builder.AddCompanyLine(companyLine);
        builder.AddTable(table);
    }

    public static void FillBuilderByData(GrantedMajorUsedOrNonUsedExcelReportBuilder builder)
    {
        var companyLine = new ExcelReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var filterSection = new ExcelReportFilterSection("Filter",
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Labore molestiae ipsam nemo iure! Recusandae nulla, fugiat ad voluptatibus impedit similique laboriosam tenetur alias! Sunt magni porro veritatis quos, laborum fugiat.");


        var subTables = GetExcelTable();

        var table = new GrantedMajorUsedOrNonUsedExcelReportTable(subTables, null);


        builder.AddCompanyLine(companyLine);
        builder.AddFilterSection(filterSection);
        builder.AddTable(table);
    }
}