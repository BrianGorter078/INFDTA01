using System;
using System.Linq;

namespace ConsoleApplication {

    public class Supply {
        public double Demand {get; set;}
        public int Month {get; set;}

        public Supply() {
            
        }
        public Supply(int month, double demand) {
            Month = month;
            Demand = demand;
        }
    }

}