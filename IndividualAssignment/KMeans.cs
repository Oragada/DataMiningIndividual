using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndividualAssignment
{
    class KMeans
    {
        private List<double[]> dataset;
        private List<KMCluster> clusters; 
        private Random rand = new Random(1337);

        public KMeans(List<double[]> dataset, int clusterSize)
        {
            this.dataset = dataset;
            clusters = new List<KMCluster>();

            int[] startIndexes = new int[clusterSize];
            //initialising
            for (int i = 0; i < startIndexes.Length; i++)
            {
                startIndexes[i] = -1;
                while (startIndexes[i] == -1)
                {
                    int num = rand.Next(0, dataset.Count);
                    if(startIndexes.Contains(num))
                    {
                        continue;
                    }

                    startIndexes[i] = num;

                }
                
            }

            foreach (int t in startIndexes)
            {
                clusters.Add(new KMCluster(dataset[t]));
            }

            //KMeanAlgorithm();
        }

        public List<KMCluster> KMeanAlgorithm()
        {
            bool changed;
            do
            {
                changed = false;
                foreach (KMCluster kmCluster in clusters)
                {
                    kmCluster.clusterContent = new List<double[]>();
                }

                //Assign all iris to empty clusters
                foreach (double[] dataPoint in dataset)
                {
                    KMCluster nearestCluster =
                        clusters.Aggregate(
                            (minCluster, thisCluster) =>
                            (calculateDistance(minCluster.center, dataPoint) <
                             calculateDistance(thisCluster.center, dataPoint)
                                 ? minCluster
                                 : thisCluster));
                    nearestCluster.InsertDataPoint(dataPoint);
                }

                //clusters now filled
                //TODO check that each datapoint is in a cluster

                //Generate new clusters
                List<KMCluster> newClusters = new List<KMCluster>();

                foreach (KMCluster kmCluster in clusters)
                {
                    KMCluster nCluster = kmCluster.GenerateNewCluster();
                    //Check changed
                    if (!kmCluster.Equals(nCluster))
                    {
                        changed = true;
                    }
                    newClusters.Add(nCluster);
                }
                if (changed) { clusters = newClusters; }
                
            } //if changed, run method again
            while (changed);

            return clusters;
        }

        private double calculateDistance(double[] center, double[] dataPoint)
        {
            //assume that the arrays are of equal length
            double val = 0;
            for (int i = 0; i < center.Length; i++)
            {
                val += Math.Pow(center[i] - dataPoint[i], 2);
            }
            return Math.Sqrt(val);
        }
    }

    class KMCluster : IEquatable<KMCluster>
    {
        public readonly double[] center;
        public List<double[]> clusterContent;

        public KMCluster(double[] center)
        {
            this.center = center;
            clusterContent = new List<double[]>();
        }

        public KMCluster GenerateNewCluster()
        {
            return new KMCluster(GenerateNewCenter());
        }

        private double[] GenerateNewCenter()
        {
            double[] newCenter = new double[center.Length];
            foreach (double[] doublese in clusterContent)
            {
                for (int i = 0; i < doublese.Length; i++)
                {
                    newCenter[i] += doublese[i];
                }
            }

            for (int i = 0; i < newCenter.Length; i++)
            {
                newCenter[i] /= clusterContent.Count;
            }

            return newCenter;
        }

        public bool Equals(KMCluster other)
        {
            return !center.Where((t, i) => Math.Abs(t - other.center[i]) > 0.0001).Any();
        }

        public void InsertDataPoint(double[] dataPoint)
        {
            clusterContent.Add(dataPoint);
        }
    }
}
