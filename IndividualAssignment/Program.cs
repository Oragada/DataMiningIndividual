using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IndividualAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string[]> data = new List<string[]>();

            var reader = new StreamReader(File.OpenRead(@"../../Data_Mining_Student_DataSet_Spring_2013_Fixed.csv"));

            string colList = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(';');
                //If there is not the required number of fields, the data point is discarded
                if (values.Length != 26) continue; 
                data.Add(values.Take(25).ToArray());

            }

            //foreach (string[] stringse in data)
            //{
            //    Console.WriteLine(stringse[17]);
            //}

            Console.ReadKey();

            List<CleanDataPoint> cleanData = data.Select(CleanDataPoint.Clean).ToList();

        }
    }
}
