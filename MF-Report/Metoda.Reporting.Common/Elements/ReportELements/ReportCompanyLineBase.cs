using System;

namespace Metoda.Reporting.Common.Elements.ReportElements;

public abstract class ReportCompanyLineBase<TContainer> : ReportElement<TContainer>
    where TContainer : class
{
    protected readonly string _companyName;
    protected readonly DateTime _referenceDate;

    public float FontSize { get; set; } = 11f;

    public ReportCompanyLineBase(string companyName, DateTime referenceDate)
    {
        _companyName = companyName;
        _referenceDate = referenceDate;
    }
}