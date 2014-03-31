﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndividualAssignment
{
    public class kNN
    {
        public static Dictionary<Field[], ClassLabel> Training = new Dictionary<Field[], ClassLabel>
            {
                {(new Field[] {new NumericField(0.3), new NumericField(0.5), new NumericField(0.1), new NominalField(4)}), ClassLabel.One},
                {(new Field[] {new NumericField(0.7), new NumericField(0.2), new NumericField(0.7), new NominalField(4)}), ClassLabel.One},
                {(new Field[] {new NumericField(0.1), new NumericField(0.4), new NumericField(0.2), new NominalField(8)}), ClassLabel.Two},
                {(new Field[] {new NumericField(0.5), new NumericField(0.8), new NumericField(0.0), new NominalField(4)}), ClassLabel.One},
                {(new Field[] {new NumericField(0.2), new NumericField(0.6), new NumericField(0.2), new NominalField(8)}), ClassLabel.Two},
                {(new Field[] {new NumericField(0.6), new NumericField(0.9), new NumericField(0.1), new NominalField(4)}), ClassLabel.One},
                {(new Field[] {new NumericField(0.8), new NumericField(0.4), new NumericField(0.1), new NominalField(8)}), ClassLabel.Two},
                {(new Field[] {new NumericField(0.3), new NumericField(0.6), new NumericField(0.2), new NominalField(4)}), ClassLabel.One},
                {(new Field[] {new NumericField(0.5), new NumericField(0.4), new NumericField(0.3), new NominalField(4)}), ClassLabel.One},
                {(new Field[] {new NumericField(0.1), new NumericField(0.9), new NumericField(0.7), new NominalField(8)}), ClassLabel.Two},
                {(new Field[] {new NumericField(1.0), new NumericField(0.5), new NumericField(0.5), new NominalField(8)}), ClassLabel.Two}
            };

        public static Dictionary<Field[], ClassLabel> Test = new Dictionary<Field[], ClassLabel>
            {
                {(new Field[] {new NumericField(0.4), new NumericField(0.8), new NumericField(0.4), new NominalField(4)}), ClassLabel.One},
                {(new Field[] {new NumericField(0.5), new NumericField(0.0), new NumericField(0.9), new NominalField(8)}), ClassLabel.Two}
            };

        readonly Random rand = new Random();

        public Dictionary<Field[], ClassLabel> Data;

        public kNN()
        {
            Data = new Dictionary<Field[], ClassLabel>();
        }

        public void AddTrainingTuple(Field[] dataPoint, ClassLabel label)
        {
            Data.Add(dataPoint,label);
        }

        public ClassLabel TestTuple(int k, Field[] dataPoint)
        {
            List<Field[]> nearest = FindKNearestNeighbors(k, dataPoint);
            List<KeyValuePair<Field[], ClassLabel>> stuff = Data.Where(e => nearest.Contains(e.Key)).ToList();
            List<ClassLabel> labels = stuff.Select(e => e.Value).ToList(); 
            Dictionary<ClassLabel, int> counts = new Dictionary<ClassLabel, int>();

            //Find count of each label
            foreach (ClassLabel label in labels)
            {
                if (counts.ContainsKey(label))
                {
                    counts[label]++;
                }
                else
                {
                    counts.Add(label, 1);
                }
            }
            //Find labels with maximum
            List<ClassLabel> maxLabels = counts.Where(e => e.Value == counts.Values.Max()).Select(e => e.Key).ToList();
            //If multiple maximums, select one at random
            return maxLabels.Count() > 1 ? maxLabels[rand.Next(0, maxLabels.Count)] : maxLabels.First();
        }

        List<Field[]> FindKNearestNeighbors(int k, Field[] newDataPoint)
        {
            Dictionary<Field[], double> distances = Data.Keys.ToDictionary(e => e, dp => FindDistance(dp, newDataPoint));
            //find the k lowest values
            KeyValuePair<Field[], double>[] lowest = new KeyValuePair<Field[], double>[k];
            for (int i = 0; i < k; i++)
            {
                lowest[i] = new KeyValuePair<Field[], double>(new Field[0], -1);
            }

            foreach (KeyValuePair<Field[], double> investigatedPoint in distances)
            {
                for (int i = 0; i < k; i++)
                {
                    if (lowest[i].Value < 0)
                    {
                        lowest[i] = investigatedPoint;
                        break;
                    }
                }
                for (int i = 0; i < k; i++)
                {
                    if (lowest[i].Value > investigatedPoint.Value)
                    {
                        lowest[i] = investigatedPoint;
                        break;
                    }
                }
            }

            return lowest.Select(e => e.Key).ToList();
        }

        private static double FindDistance(Field[] first, Field[] second)
        {
            double sum = 0;
            for (int i = 0; i < first.Length; i++)
            {
                sum += Math.Pow(Math.Abs(first[i].GetDistance(second[i])),2);
            }
            return Math.Sqrt(sum);
        }
    }
}
