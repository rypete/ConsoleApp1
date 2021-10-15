using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ResidentTaxDeduplicator residentTaxDeduplicator = new ResidentTaxDeduplicator();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            AllTaxes taxes = JsonConvert.DeserializeObject<AllTaxes>(File.ReadAllText("alltaxes.json"));
            Console.WriteLine("Count Start: " + taxes.Taxes.Count);

            residentTaxDeduplicator.Deduplicate(taxes.Taxes);
            sw.Stop();

            Console.WriteLine("Time: " + sw.Elapsed);
        }
    }
}
