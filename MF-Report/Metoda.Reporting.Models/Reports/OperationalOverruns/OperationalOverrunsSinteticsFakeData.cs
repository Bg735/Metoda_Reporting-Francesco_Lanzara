using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Excel.ReportElements;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements;
using System.Collections.Generic;
using System;

namespace Metoda.Reporting.Models.Reports.OperationalOverruns;

public static class OperationalOverrunsSinteticsFakeData
{
    public static string[] _fenomenos = new[] { "550200", "550300", "550400" };
    public static string[] _descrizionos = new[]
        {
                    "Crediti per cassa – Rischio Autoliquidante 1",
                    "Crediti per cassa – Rischio Autoliquidante 2",
                    "Crediti per cassa – Rischio Autoliquidante 3"
                };

    private static void FillSubtables(out List<OperationalOverrunsSinteticsItem> subTable, out TotalRow<OperationalOverrunsSinteticsItem> totalRow)
    {
        Random random = new();

        subTable = new List<OperationalOverrunsSinteticsItem>();

        for (int k = 0; k < _fenomenos.Length; k++)
        {
            subTable.Add(new OperationalOverrunsSinteticsItem
            {
                Fenomeno = _fenomenos[k],
                Descrizione = _descrizionos[k],
                Accordato_31 = random.Next(4000, 10000),
                Utilizzato_33 = random.Next(1000, 4000),
                AccOperativo_32 = random.Next(1000, 4000),
                SconfDeliberato_33_31 = random.Next(1000, 4000),
                SconfOperativo_33_32 = random.Next(1000, 4000),
            });
        }

        totalRow = null;
    }

    public static IList<ExcelTable<OperationalOverrunsSinteticsItem>> GetExcelTable()
    {
        var list = new List<ExcelTable<OperationalOverrunsSinteticsItem>>();

        FillSubtables(out List<OperationalOverrunsSinteticsItem> subTable, out TotalRow<OperationalOverrunsSinteticsItem> totalRow);

        var res = new ExcelTable<OperationalOverrunsSinteticsItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static IList<PdfTable<OperationalOverrunsSinteticsItem>> GetPdfTable()
    {
        var list = new List<PdfTable<OperationalOverrunsSinteticsItem>>();

        FillSubtables(out List<OperationalOverrunsSinteticsItem> subTable, out TotalRow<OperationalOverrunsSinteticsItem> totalRow);

        var res = new PdfTable<OperationalOverrunsSinteticsItem>(subTable, totalRow);

        list.Add(res);
        return list;
    }

    public static void FillBuilderByData(OperationalOverrunsSinteticsPdfReportBuilder builder)
    {
        var companyLine = new PdfReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var subTables = GetPdfTable();

        var table = new OperationalOverrunsSinteticsPdfReportTable(subTables, null);

        builder.AddCompanyLine(companyLine);
        builder.AddTable(table);
    }

    public static void FillBuilderByData(OperationalOverrunsSinteticsExcelReportBuilder builder)
    {
        var companyLine = new ExcelReportCompanyLine
            (
                "09999 - Banca di Prova",
                new DateTime(2018, 09, 30)
            );

        var filterSection = new ExcelReportFilterSection("Filter",
        "Lorem ipsum dolor sit amet consectetur adipisicing elit. Labore molestiae ipsam nemo iure! Recusandae nulla, fugiat ad voluptatibus impedit similique laboriosam tenetur alias! Sunt magni porro veritatis quos, laborum fugiat.");


        var subTables = GetExcelTable();

        var table = new OperationalOverrunsSinteticsExcelReportTable(subTables, null);


        builder.AddCompanyLine(companyLine);
        builder.AddFilterSection(filterSection);
        builder.AddTable(table);
    }
}