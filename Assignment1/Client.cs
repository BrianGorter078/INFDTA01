using System;
using System.Linq;
using System.Collections.Generic;

namespace Assignment1
{
    public class Client
    {
        public int ClusterNumber;
        public Vector Data;

        public Client(IEnumerable<bool> wines)
        {
            Data = new Vector();

            foreach (var wine in wines)
            {
                Data.Add(wine ? 1d : 0d);
            }

            ClusterNumber = 0;
        }

        public override string ToString()
        {
            return String.Format("ClusterNumber: {0} points {1}", ClusterNumber, Data.ToString());
        }

        public double Distance(Vector other)
        {
            return Data.Distance(other);
        }
    }
}