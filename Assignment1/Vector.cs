using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace Assignment1
{
    public class Vector
    {
        public double[] Points;
        public Vector() : this(0)
        {
        }

        public Vector(int dimension)
        {
            Points = new double[dimension];
        }

        public void Add(double item)
        {
            Points = Points.Append(item).ToArray();
        }

        public void AppendValue(int index, double value)
        {
            Points[index] += value;
        }

        public double Distance(Vector other)
        {
            var sum = .0;

            if (Points.Length != other.Points.Length)
            {
                return sum;
            }
            for (int i = 0; i < Points.Length; i++)
            {
                double subtraction = other.Points[i] - Points[i];
                sum += Math.Pow(subtraction, 2);
            }

            return Math.Sqrt(sum);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var item = (Vector)obj;
            if (item.Points.Length != Points.Length)
            {
                return false;
            }

            return Points.Equals(item.Points);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return String.Join(",", Points);
        }

        public void PrettyPrint()
        {
            Console.WriteLine("[Vector] points = {0}", String.Join(",", Points.Select(p => p.ToString())));
        }
    }
}