﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IndividualAssignment
{
    class Program
    {
        /*
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
                if (values.Length != 25) continue; 
                data.Add(values.Take(25).ToArray());

            }

            //foreach (string[] stringse in data)
            //{
            //    Console.WriteLine(stringse[17]);
            //}

            List<CleanDataPoint> cleanData = data.Select(CleanDataPoint.Clean).ToList();

            Analysis ana = new Analysis();

            ana.RunAPriori(cleanData);

            PrintData(cleanData);

            Console.ReadKey();

        }

        private static void PrintData(List<CleanDataPoint> cleanData)
        {
            foreach (CleanDataPoint cleanDataPoint in cleanData)
            {
                Console.Write("{0}, {1}, {2}", cleanDataPoint.ProgLang[0], cleanDataPoint.ProgLang[1], cleanDataPoint.ProgLang[2]);
                Console.WriteLine();
            }
        }
        */

        static void Main(string[] args)
        {
            Analysis ana = new Analysis();
            ana.RunKNN(new List<CleanDataPoint>());
        }
    }
}
