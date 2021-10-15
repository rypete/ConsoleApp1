using System;
using System.Collections.Generic;

public class ResidentTaxDeduplicator
{
    public IEnumerable<TaxItem> Deduplicate(List<TaxItem> taxes)
    {
        List<TaxItem> mergedTaxes = new List<TaxItem>();
        List<TaxItem> residentialTaxes = new List<TaxItem>();
        Dictionary<string, TaxItem> nonResidentialTaxes = new Dictionary<string, TaxItem>();
        int count = 0;

        // Iterates over the entire collection and sort by residential / nonResidential taxes.
        foreach (TaxItem currentTax in taxes)
        {
            count += 1;
            string Key = CreateKey(currentTax);
            bool nonResidentialTaxExists = nonResidentialTaxes.TryGetValue(Key, out TaxItem nonResidentialTax);

            // Cheap shot - lets check if we get lucky and the nonResidential half has already been processed.
            if (nonResidentialTaxExists)
            {
                currentTax.NonResidentRate = nonResidentialTax.Rate;
                nonResidentialTaxes.Remove(Key);
                mergedTaxes.Add(currentTax);

                continue;
            }

            if (currentTax.IsResidentTax)
            {
                residentialTaxes.Add(currentTax);
            }
            else
            {
                nonResidentialTaxes.Add(Key, currentTax);
            }
        }

        // Iterate over the residential taxes and try to get the nonResidential half.
        // If found, merge and delete from nonResidentialTaxes dict.
        foreach (TaxItem currentTax in residentialTaxes)
        {
            count += 1;
            string Key = CreateKey(currentTax);
            bool nonResidentialTaxExists = nonResidentialTaxes.TryGetValue(Key, out TaxItem nonResidentialTax);

            if (nonResidentialTaxExists)
            {
                currentTax.NonResidentRate = nonResidentialTax.Rate;
                nonResidentialTaxes.Remove(Key);
            }

            mergedTaxes.Add(currentTax);
        }

        // Add any unmatched nonResidentialTaxes in case the residential half didnt exist.
        foreach (TaxItem currentTax in nonResidentialTaxes.Values)
        {
            count += 1;
            mergedTaxes.Add(currentTax);
        }

        Console.WriteLine("Operations: "+ count);
        Console.WriteLine("Count End: " + mergedTaxes.Count);

        return mergedTaxes;
    }

    private string CreateKey(TaxItem tax)
    {
        return $"{tax.TaxCode}-{tax.SecondaryTaxType}";
    }
}
