using Metoda.Reporting.Common.Elements.Contracts;
using Metoda.Reporting.Common.Elements.Table;
using Metoda.Reporting.Common.Enums;
using Metoda.Reporting.Excel.ReportElements.Tables;
using Metoda.Reporting.Pdf.ReportElements.Tables;
using System.Collections.Generic;

namespace Metoda.Reporting.Models.Reports.TypeOfActivityIncompatibleWithRisksAtMaturity;

public class TypeOfActivityIncompatibleWithRisksAtMaturityPdfReportTable
        : PdfReportMultipleTableBase<TypeOfActivityIncompatibleWithRisksAtMaturityItem>
{
    public TypeOfActivityIncompatibleWithRisksAtMaturityPdfReportTable(
        IList<PdfTable<TypeOfActivityIncompatibleWithRisksAtMaturityItem>> tables,
        TotalRow<TypeOfActivityIncompatibleWithRisksAtMaturityItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}

public class TypeOfActivityIncompatibleWithRisksAtMaturityExcelReportTable
    : ExcelReportMultipleTableBase<TypeOfActivityIncompatibleWithRisksAtMaturityItem>
{
    public TypeOfActivityIncompatibleWithRisksAtMaturityExcelReportTable(
        IList<ExcelTable<TypeOfActivityIncompatibleWithRisksAtMaturityItem>> tables,
        TotalRow<TypeOfActivityIncompatibleWithRisksAtMaturityItem> mainTotalRow,
        string title = null,
        IntermediateTotalLocation totalLocation = IntermediateTotalLocation.TableBottom,
        IReportProgress progress = null
        ) : base(tables, mainTotalRow, title, totalLocation, progress)
    {
    }
}