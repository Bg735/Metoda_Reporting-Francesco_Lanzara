using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metoda.Reporting.Models.Reports.TypeOfActivityIncompatibleWithRisksAtMaturity;

public static class TypeOfActivityIncompatibleWithRisksAtMaturityFakeData
{
    private static void FillSubtables(
        out List<TypeOfActivityIncompatibleWithRisksAtMaturityItem> subTable, 
        out TotalRow<TypeOfActivityIncompatibleWithRisksAtMaturityItem> totalRow)
    {
        var centsitos = new[] { "12345 - Soggetto A", "45687 - Soggetto A", "77295 - Soggetto B" };

        Random random = new();
        decimal accordato, utilizzato;

        subTable = new List<TypeOfActivityIncompatibleWithRisksAtMaturityItem>();
        for (int k = 0; k < centsitos.Length; k++)
        {
            accordato = random.Next(4000, 10000);
            utilizzato = random.Next(1000, 4000);

            subTable.Add(new TypeOfActivityIncompatibleWithRisksAtMaturityItem
            {
                Accordato = accordato,
                CodCensito = centsitos[k],
                Fenomeno = "550400 - <descrizione>",
                TipoAttivita = "25 - <descrizione>",
                Utilizzato = utilizzato
            });
        }

        totalRow = new TotalRow<TypeOfActivityIncompatibleWithRisksAtMaturityItem>(
            new TypeOfActivityIncompatibleWithRisksAtMaturityItem
            {
                Accordato = subTable.Select(_ => _.Accordato).Sum(),
                Utilizzato = subTable.Select(_ => _.Utilizzato).Sum()
            });
    }

    public static IList<ExcelTable<TypeOfActivityIncompatibleWithRisksAtMaturityItem>> GetExcelTable()
    {
        var list = new List<ExcelTable<TypeOfActivityIncompatibleWithRisksAtMaturityItem>>();

        FillSubtables(out List<TypeOfActivityIncompatibleWithRisksAtMaturityItem> subTable, out TotalRow<TypeOfActivityIncompatibleWithRisksAtMaturityItem> totalRow);

        var res = new ExcelTable<TypeOfActivityIncompatibleWithRisksAtMaturityItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static IList<PdfTable<TypeOfActivityIncompatibleWithRisksAtMaturityItem>> GetPdfTable()
    {
        var list = new List<PdfTable<TypeOfActivityIncompatibleWithRisksAtMaturityItem>>();

        FillSubtables(out List<TypeOfActivityIncompatibleWithRisksAtMaturityItem> subTable, out TotalRow<TypeOfActivityIncompatibleWithRisksAtMaturityItem> totalRow);

        var res = new PdfTable<TypeOfActivityIncompatibleWithRisksAtMaturityItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static void FillBuilderByData(TypeOfActivityIncompatibleWithRisksAtMaturityPdfReportBuilder builder)
    {
        var companyLine = new PdfReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var subTables = GetPdfTable();

        var table = new TypeOfActivityIncompatibleWithRisksAtMaturityPdfReportTable(subTables, null);//, "Table title");

        builder.AddCompanyLine(companyLine);
        builder.AddTable(table);
    }

    public static void FillBuilderByData(TypeOfActivityIncompatibleWithRisksAtMaturityExcelReportBuilder builder)
    {
        var companyLine = new ExcelReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var filterSection = new ExcelReportFilterSection("Filter",
        @"Lorem ipsum dolor sit amet consectetur adipisicing elit. 
            Labore molestiae ipsam nemo iure! Recusandae nulla, fugiat ad voluptatibus 
            impedit similique laboriosam tenetur alias! Sunt magni porro veritatis quos, laborum fugiat.");

        var subTables = GetExcelTable();

        var table = new TypeOfActivityIncompatibleWithRisksAtMaturityExcelReportTable(subTables, null);//, "Table title");


        builder.AddCompanyLine(companyLine);
        builder.AddFilterSection(filterSection);
        builder.AddTable(table);
    }
}
