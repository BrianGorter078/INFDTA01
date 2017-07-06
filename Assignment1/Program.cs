using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Assignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("./WineData.csv");

            var winesBoughtByCustomers = lines.Select(line => 
                line.Split(',').Select(
                    column => column == "1"
                    ).ToArray()
            ).ToArray(); 

            var wineCount = winesBoughtByCustomers.Count();
            var customerCount = winesBoughtByCustomers.First().Count();

            var customers = Enumerable
                    .Range(0, customerCount)
                    .Select(customerIndex => new Client(winesBoughtByCustomers.Select(wines => wines[customerIndex])))
                    .ToList();
                    
            var kmeans = new KMeans();
            var KclusterNumberes = 4;
            Tuple<Client[],double> result = Tuple.Create(new Client[101010],0.0);
            double lowestSSE = double.PositiveInfinity;

            for (var i = 0; i < 30; i++){
                var kmeansResult = kmeans.Run(customers.ToArray(),KclusterNumberes,10);

                if(kmeansResult.Item2 < lowestSSE){
                    lowestSSE = kmeansResult.Item2;
                    result = kmeansResult;
                }
            }


            for (int i = 0; i < KclusterNumberes; i++)
            {
                Console.WriteLine("Cluster {0}", i + 1);
                printCluster(result.Item1.Where(c => c.ClusterNumber == i), wineCount);
                Console.WriteLine("----------------");
            }
            Console.WriteLine("SSE: {0}", result.Item2);
        }
        public static void printCluster(IEnumerable<Client> clients, int wineCount)
        {

            if (!clients.Any())
            {
                return;
            }

            var countByWines = Enumerable
                .Range(0, wineCount)
                .Select(wine => new { Wine = wine, Count = clients.Count(c => c.Data.Points[wine] == 1d) })
                .OrderByDescending(countPerWine => countPerWine.Count)
                .ToList();

            foreach (var countPerWine in countByWines)
            {
                Console.WriteLine(String.Format("Offer {0} => bought {1} times", countPerWine.Wine, countPerWine.Count));
            }
        }
    }
}
