using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var smoothing = new Smoothing();
                  
            var lines = File.ReadAllLines("./swords.csv");

            var supplyList = lines.Select((line) => {
                var columns = line.Split(',');
                if (columns.Length < 2) {
                    return new Supply();
                }
                var month = Int32.Parse(columns[0]);
                var demand = Double.Parse(columns[1]);

                return new Supply(month, demand);
            }).ToList();

            var demands = supplyList.Select(s => s.Demand );
            var bestSmoothingFactorSES = double.PositiveInfinity;
            var bestSmoothingFactorDES = double.PositiveInfinity;

            var sesResult = new Result();
            var desResult = new Result();

            // Simple exponential smoothing
            for (var alpha = 0.1; alpha < 0.9; alpha+= 0.1)
            {   
                var smoothedList = smoothing.SimpleExponentialSmoothing(demands, alpha);
                var forecast = smoothing.SESForeCasting(demands, smoothedList, 12, alpha);
                var sse = Math.Sqrt(smoothing.SSE(smoothedList, demands, 1));
                if (sse < bestSmoothingFactorSES)
                {
                    bestSmoothingFactorSES = sse;
                    sesResult = new Result(forecast, demands, smoothedList, alpha, 0, sse);
                }
            }

            // Double exponential smoothing
            for (var alpha = 0.1; alpha < 0.9; alpha += 0.1)
            {
                for (var beta = 0.1; beta < 0.9; beta += 0.1)
                {
                    var smoothedList = smoothing.DoubleExponentialSmoothing(demands, alpha, beta);
                    var forecast = smoothing.DESForeCasting(demands, smoothedList.Item1, 12, alpha, beta, smoothedList.Item2);
                    var sse = forecast.Item2;

                    if (sse < bestSmoothingFactorDES)
                    {
                        bestSmoothingFactorDES = sse;
                        desResult = new Result(forecast.Item1, demands, smoothedList.Item1, alpha, beta, sse);
                    }
                }
            }

            var sesJson = JsonConvert.SerializeObject(sesResult);
            var desJson = JsonConvert.SerializeObject(desResult);

            Console.WriteLine("Ses result: \n" + sesJson + "\n---------------------------------\n");
            Console.WriteLine("des result: \n" + desJson + "\n---------------------------------\n");

            File.WriteAllText("ses.json", sesJson);
            File.WriteAllText("des.json", desJson);

            Console.ReadLine();
        }
    }
}
