using System;


namespace Assignment1
{
    public class CentroidItem
    {
        public int Number;
        public Vector Data;

        public CentroidItem(Vector v, int number)
        {
            Data = v;
            Number = number;
        }

        public override bool Equals(object obj)
        {
            var centroidData = (CentroidItem)obj;

            return Data.Equals(centroidData.Data);
        }
    }
}