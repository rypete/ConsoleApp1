using System;
public class TaxItem
{
    public string TaxCode;
    public string Description;
    public bool IsEmployerTax;
    public bool IsResidentTax;
    public decimal? Rate;
    public decimal? WageBase;
    public string TaxType;
    public string SecondaryTaxType;
    public string StateCode;
    public DateTime EffectiveDate;
    public decimal TaxLimitAmount;
    public string LocationCode;
    public bool IsFromResidentLookup;
    public decimal? NonResidentRate;
}