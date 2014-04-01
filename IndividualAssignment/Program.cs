using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IndividualAssignment
{
    class Program
    {
        /*
        //A Priori
        static void Main(string[] args)
        {
            List<string[]> data = ExtractData();

            List<CleanDataPoint> cleanData = data.Select(CleanDataPoint.Clean).ToList();

            Analysis ana = new Analysis();

            ana.RunAPriori(cleanData);

            //PrintProgLangs(cleanData);

            Console.ReadKey();
        }
        */
        
        //kNN
        static void Main(string[] args)
        {
            List<string[]> data = ExtractData();

            List<CleanDataPoint> cleanData = data.Select(CleanDataPoint.Clean).ToList();

            Analysis ana = new Analysis();
          
            ana.RunKNN(cleanData);
        }
        
        /*
        //K-Means
        static void Main(string[] args)
        {
            List<string[]> data = ExtractData();

            List<CleanDataPoint> cleanData = data.Select(CleanDataPoint.Clean).ToList();

            Analysis ana = new Analysis();
            
            ana.RunKMeans(cleanData);
        }
        */

        private static List<string[]> ExtractData()
        {
            List<string[]> data = new List<string[]>();

            var reader = new StreamReader(File.OpenRead(@"../../Data_Mining_Student_DataSet_Spring_2013_Fixed.csv"));

            string colList = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(';');
                //If there is not the required number of fields, the data point is discarded
                if (values.Length != 25) continue;
                data.Add(values.Take(25).ToArray());
            }

            return data;
        }

        private static void PrintProgLangs(List<CleanDataPoint> cleanData)
        {
            foreach (CleanDataPoint cleanDataPoint in cleanData)
            {
                Console.Write("{0}, {1}, {2}", cleanDataPoint.ProgLang[0], cleanDataPoint.ProgLang[1], cleanDataPoint.ProgLang[2]);
                Console.WriteLine();
            }
        }
    }
}
