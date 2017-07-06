using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment1
{
    public class KMeans
    {
        public Tuple<Client[], double> Run(Client[] clients, int MaxAmountOfClusters, int MaxAmountOfIterations)
        {
            var centroidChooserList = clients.ToList();

            if (clients.Length < MaxAmountOfClusters)
            {
                return null;
            }
            var changed = true;
            var centroids = new List<CentroidItem>();

            Random rnd = new Random();

            for (int i = 0; i < MaxAmountOfClusters; i++)
            {
                int randomIndex = rnd.Next(centroidChooserList.Count);
                var data = centroidChooserList[randomIndex].Data;
                centroidChooserList = centroidChooserList.Where(x => !x.Data.Equals(data)).ToList();
                centroids.Add(new CentroidItem(data, i));
            }

            while (changed && MaxAmountOfIterations != 0)
            {
                foreach (var client in clients)
                {
                    double lowestDistance = double.PositiveInfinity;

                    for (int i = 0; i < MaxAmountOfClusters; i++)
                    {
                        double distanceBetweenPoints = client.Distance(centroids[i].Data);
                        if (distanceBetweenPoints < lowestDistance)
                        {
                            lowestDistance = distanceBetweenPoints;
                            client.ClusterNumber = centroids[i].Number;
                        }
                    }
                }

                var centroidWithPoints = clients
                    .GroupBy(c => c.ClusterNumber)
                    .ToDictionary(c => c.Key, c => c.ToList());

                
                var centroidNotChangedCount = 0;
                foreach (var c in centroidWithPoints)
                {
                    var oldCentroid = centroids.First(x => x.Number == c.Key);

                    var list = new List<Vector>();
                    foreach (var client in c.Value)
                    {
                        list.Add(client.Data);
                    }

                    var newCentroid = new CentroidItem(Mean(list, oldCentroid.Data.Points.Length), c.Key);

                    if (!oldCentroid.Equals(newCentroid))
                    {
                        centroids[newCentroid.Number] = newCentroid;
                    }
                    else
                    {
                        centroidNotChangedCount++;
                    }
                }

                if(centroidNotChangedCount == MaxAmountOfClusters) {
                    changed = false;
                }

                MaxAmountOfIterations--;
            }

            var sse = .0;
            for (int i = 0; i < MaxAmountOfClusters; i++)
            {
                var clientByClusterId = clients.Where(x => x.ClusterNumber == i);
                sse += SquaredError(centroids[i], clientByClusterId);
            }

            return Tuple.Create(clients, sse);
        }

        public double SquaredError(CentroidItem centroid, IEnumerable<Client> PointsInCluster)
        {
            double amount = .0;
            foreach (var point in PointsInCluster)
            {
                amount += Math.Pow(point.Distance(centroid.Data), 2);
            }
            return amount;
        }

        public Vector Mean(List<Vector> points, int length)
        {
            var newVector = new Vector(length);

            foreach (var vector in points)
            {
                for (int i = 0; i < newVector.Points.Length; i++)
                {
                    newVector.AppendValue(i, vector.Points[i]);
                }
            }

            for (int i = 0; i < newVector.Points.Length; i++)
            {
                newVector.Points[i] = (double)newVector.Points[i] / points.Count;
            }

            return newVector;
        }
    }
}