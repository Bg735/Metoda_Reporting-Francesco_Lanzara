using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements;
using System.Collections.Generic;
using System;

namespace Metoda.Reporting.Models.Reports.OperationalOverruns;


public static class OperationalOverrunsAnaliticsFakeData
{
    public static string[] _centsitos = new[] { "12345 - Soggetto A", "45687 - Soggetto A", "77295 - Soggetto B" };
    public static string[] _fenomenos = new[] { "550200", "550300", "550400" };
    public static string[] _ndgs = new[] { "12345", "54682", "75124" };
    public static string[] _descrizionos = new[]
        {
                "Crediti per cassa – Rischio Autoliquidante 1",
                "Crediti per cassa – Rischio Autoliquidante 2",
                "Crediti per cassa – Rischio Autoliquidante 3"
            };

    private static void FillSubtables(out List<OperationalOverrunsAnaliticsItem> subTable, out TotalRow<OperationalOverrunsAnaliticsItem> totalRow)
    {
        Random random = new();

        subTable = new List<OperationalOverrunsAnaliticsItem>();

        for (int k = 0; k < _centsitos.Length; k++)
        {
            subTable.Add(new OperationalOverrunsAnaliticsItem
            {
                Fenomeno = _fenomenos[k],
                Descrizione = _descrizionos[k],
                Accordato_31 = random.Next(4000, 10000),
                Utilizzato_33 = random.Next(1000, 4000),
                AccOperativo_32 = random.Next(1000, 4000),
                SconfDeliberato_33_31 = random.Next(1000, 4000),
                SconfOperativo_33_32 = random.Next(1000, 4000),
                CodCensito = _centsitos[k],
                NDG = _ndgs[k]
            });
        }

        totalRow = null;
    }

    public static IList<ExcelTable<OperationalOverrunsAnaliticsItem>> GetExcelTable()
    {
        var list = new List<ExcelTable<OperationalOverrunsAnaliticsItem>>();

        FillSubtables(out List<OperationalOverrunsAnaliticsItem> subTable, out TotalRow<OperationalOverrunsAnaliticsItem> totalRow);

        var res = new ExcelTable<OperationalOverrunsAnaliticsItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static IList<PdfTable<OperationalOverrunsAnaliticsItem>> GetPdfTable()
    {
        var list = new List<PdfTable<OperationalOverrunsAnaliticsItem>>();

        FillSubtables(out List<OperationalOverrunsAnaliticsItem> subTable, out TotalRow<OperationalOverrunsAnaliticsItem> totalRow);

        var res = new PdfTable<OperationalOverrunsAnaliticsItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static void FillBuilderByData(OperationalOverrunsAnaliticsPdfReportBuilder builder)
    {
        var companyLine = new PdfReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var subTables = GetPdfTable();

        var table = new OperationalOverrunsAnaliticsPdfReportTable(subTables, null);

        builder.AddCompanyLine(companyLine);
        builder.AddTable(table);
    }

    public static void FillBuilderByData(OperationalOverrunsAnaliticsExcelReportBuilder builder)
    {
        var companyLine = new ExcelReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var filterSection = new ExcelReportFilterSection("Filter",
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Labore molestiae ipsam nemo iure! Recusandae nulla, fugiat ad voluptatibus impedit similique laboriosam tenetur alias! Sunt magni porro veritatis quos, laborum fugiat.");


        var subTables = GetExcelTable();

        var table = new OperationalOverrunsAnaliticsExcelReportTable(subTables, null);


        builder.AddCompanyLine(companyLine);
        builder.AddFilterSection(filterSection);
        builder.AddTable(table);
    }
}
