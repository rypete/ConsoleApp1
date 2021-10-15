using System;
using System.Collections.Generic;

public class ResidentTaxDeduplicator
{
    public IEnumerable<TaxItem> Deduplicate(List<TaxItem> taxes)
    {
        Dictionary<string, TaxItem> mergedTaxes = new Dictionary<string, TaxItem>();
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
                mergedTaxes.Add(Key, currentTax);

                continue;
            }

            if (currentTax.IsResidentTax)
            {
                mergedTaxes.Add(Key, currentTax);
            }
            else
            {
                nonResidentialTaxes.Add(Key, currentTax);
            }
        }

        // Iterate over the nonresidential taxes and try to get the residential half.
        // If found set the NonResidentialTax field.
        // If not found, add the NonResidential Tax to the merged array.
        foreach (TaxItem currentTax in nonResidentialTaxes.Values)
        {
            count += 1;
            string Key = CreateKey(currentTax);
            bool residentialTaxExists = mergedTaxes.TryGetValue(Key, out TaxItem residentialTax);

            if (residentialTaxExists)
            {
                residentialTax.NonResidentRate = currentTax.Rate;
            } else
            {
                mergedTaxes.Add(Key, currentTax);
            }
        }

        Console.WriteLine("Operations: "+ count);
        Console.WriteLine("Count End: " + mergedTaxes.Count);

        return mergedTaxes.Values;
    }

    private string CreateKey(TaxItem tax)
    {
        return $"{tax.TaxCode}-{tax.SecondaryTaxType}";
    }
}
