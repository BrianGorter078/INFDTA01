using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ConsoleApplication {
    public class Result
    {
        [JsonProperty("smoothening_factor")]
        public double SmootheningFactor { get; set; }

        [JsonProperty("beta_factor")]
        public double BetaFactor { get; set; }

        [JsonProperty("smoothened_list")]
        public IEnumerable<double> SmoothenedList { get; set; }

        [JsonProperty("original_list")]
        public IEnumerable<double> OriginalList { get; set; }

        [JsonProperty("smoothed_values")]
        public IEnumerable<double> SmoothedValues { get; set; }

        [JsonProperty("sse")]
        public double Sse { get; set; }


        public Result()
        {

        }

        public Result(IEnumerable<double> result, IEnumerable<double> original, IEnumerable<double> smoothedValues,
            double smootheningFactor, double betaFactor, double sse)
        {
            SmootheningFactor = smootheningFactor;
            SmoothenedList = result;
            SmoothedValues = smoothedValues;
            BetaFactor = betaFactor;
            Sse = sse;
            OriginalList = original;
        }
    }
}