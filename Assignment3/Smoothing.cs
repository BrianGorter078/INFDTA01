using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Smoothing
    {
        public IEnumerable<double> SimpleExponentialSmoothing(IEnumerable<double> input, double alpha)
        {
            var demands = input.ToArray();
            var average = demands.Take(12).Average();

            List<double> smoothedValues = new List<double>();
            smoothedValues.Add(average);

            for (int i = 1; i < demands.Length; i++)
            {
                smoothedValues.Add((alpha * demands[i - 1]) + ((1 - alpha) * smoothedValues[i - 1]));
            }
            return smoothedValues;
        }

        public Tuple<List<double>, List<double>> DoubleExponentialSmoothing(IEnumerable<double> demands, double alpha, double beta)
        {
            var currentDemands = demands.ToList();
            var average = currentDemands.Take(12).Average();

            var smoothedList = new List<double>();
            smoothedList.Add(average);

            var trendList = new List<double>();
            trendList.Add(currentDemands[1] - currentDemands[0]);

            for (var i = 1; i < currentDemands.Count; i++)
            {
                var smoothedValue = (alpha * currentDemands[i]) + ((1 - alpha) * (smoothedList[i - 1] + trendList[i - 1]));
                smoothedList.Add(smoothedValue);
                var trend = (beta * (smoothedList[i] - smoothedList[i - 1])) + ((1 - beta) * trendList[i - 1]);
                trendList.Add(trend);
            }


            return Tuple.Create(smoothedList, trendList);
        }

        public Tuple<List<double>, double> DESForeCasting(IEnumerable<double> originalValues, IEnumerable<double> smoothedValues, int amountOfSteps, double alpha, double beta, List<double> trend)
        {
            var originalValuesInScope = originalValues.ToList();
            var smoothedList = smoothedValues.ToList();
            var trendList = trend.ToList();

            var squaredError = 0.0;

            var forecastList = new List<double>();
            forecastList.Add(0.0);
            forecastList.Add(0.0);

            for (int i = 2; i < originalValuesInScope.Count; i++)
            {
                var f = smoothedList[i - 1] + trendList[i - 1];
                forecastList.Add(f);

                squaredError += Math.Pow(f - originalValuesInScope[i], 2);
            }

            var finalSmoothedValue = smoothedValues.Last();
            var finalTrend = trendList.Last();

            for (int i = 1; i < amountOfSteps + 1; i++)
            {
                forecastList.Add(finalSmoothedValue + (i * finalTrend));
            }

            var sumOfSquaredError = squaredError / (originalValues.Count() - 2);
            sumOfSquaredError = Math.Sqrt(sumOfSquaredError);

            return Tuple.Create(forecastList, sumOfSquaredError);
        }

        public IEnumerable<double> SESForeCasting(IEnumerable<double> originalValues, IEnumerable<double> smoothedValues, int amountOfSteps, double alpha)
        {
            var originalValuesInScope = originalValues.ToList();
            var smoothedValuesInScope = smoothedValues.ToList();

            var smoothValue = smoothedValuesInScope.Last();

            for (int i = originalValues.Count(); i < originalValues.Count() + amountOfSteps; i++)
            {
                smoothValue = (alpha * originalValuesInScope[i - 1]) + (1 - alpha) * smoothValue;
                originalValuesInScope.Add(smoothValue);
            }

            return originalValuesInScope;
        }

        public double SSE(IEnumerable<double> smoothedValues, IEnumerable<double> demands, int subCount)
        {
            var currentDemands = demands.ToArray();
            var currentSmoothedValues = smoothedValues.ToArray();

            if (smoothedValues.Count() != demands.Count())
            {
                return .0;
            }

            var totalValue = .0;
            for (int i = 2; i < currentDemands.Length; i++)
            {
                totalValue += Math.Pow(currentSmoothedValues[i] - currentDemands[i], 2);
            }

            return totalValue / (currentDemands.Length - subCount);
        }
    }
}
