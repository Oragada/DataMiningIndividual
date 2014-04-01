using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IndividualAssignment
{
    public class Analysis
    {
        private static readonly Dictionary<OS, int> osInt = new Dictionary<OS, int>();
        private static readonly Dictionary<ProgrammingLanguage, int> progLangInt = new Dictionary<ProgrammingLanguage, int>();
        private static readonly Dictionary<SQL, int> sqlInt = new Dictionary<SQL, int>();
        private static readonly Random rand = new Random();

        private CleanDataPoint[] personData;

        public Analysis()
        {
            int i = 0;
            foreach (SQL sql in Enum.GetValues(typeof(SQL)).Cast<SQL>())
            {
                sqlInt.Add(sql, i++);
            }
            foreach (ProgrammingLanguage pL in Enum.GetValues(typeof(ProgrammingLanguage)).Cast<ProgrammingLanguage>())
            {
                progLangInt.Add(pL, i++);
            }
            foreach (OS os in Enum.GetValues(typeof(OS)).Cast<OS>())
            {
                osInt.Add(os,i++);
            }
        }

        public void RunAPriori(List<CleanDataPoint> cleanData)
        {
            List<List<int>> data = new List<List<int>>();
            foreach (CleanDataPoint cdp in cleanData)
            {
                List<int> dataPoint = new List<int>();
                dataPoint.Add(osInt[cdp.OperatingSystem]);
                dataPoint.Add(sqlInt[cdp.Server]);
                dataPoint.AddRange(cdp.ProgLang.Select(lang => progLangInt[lang]));
                data.Add(dataPoint);
            }
            Dictionary<KeyValuePair<int[], int[]>, float> results = APriori.RunAPriori(0.15f, 0.70f, data);
            WriteAPrioriResults(results);
        }

        private void WriteAPrioriResults(Dictionary<KeyValuePair<int[], int[]>, float> results)
        {
            foreach (KeyValuePair<KeyValuePair<int[], int[]>, float> k in results)
            {
                Console.WriteLine("[{0}] => [{1}], confidence {2}%", GetEnumFromInt(k.Key.Key), GetEnumFromInt(k.Key.Value),k.Value*100);
            }
        }

        private static string GetEnumFromInt(int[] arr)
        {
            StringBuilder strB = new StringBuilder();
            foreach (int i in arr)
            {
                if (strB.Length != 0)
                {
                    strB.Append(", ");
                }
                strB.Append(GetEnum(i));
            }
            return strB.ToString();
        }

        private static string GetEnum(int i)
        {
            string s = "";
            if (sqlInt.ContainsValue(i))
            {
                return sqlInt.First(e => e.Value == i).Key.ToString();
            }
            if (osInt.ContainsValue(i))
            {
                return osInt.First(e => e.Value == i).Key.ToString();
            }
            if (progLangInt.ContainsValue(i))
            {
                return progLangInt.First(e => e.Value == i).Key.ToString();
            }

            throw new InvalidDataException();
        }

        public void RunKNN(List<CleanDataPoint> cleanData)
        {
            Dictionary<Field[], ClassLabel>[] dataFields = CreateFieldsForKNN(cleanData);

            kNN knn = new kNN();

            foreach (KeyValuePair<Field[], ClassLabel> tuple in dataFields[0])
            {
                knn.AddTrainingTuple(tuple.Key,tuple.Value);
            }
            foreach (KeyValuePair<Field[], ClassLabel> tuple in dataFields[1])
            {
                ClassLabel knnResult = knn.TestTuple(3, tuple.Key);
                Console.WriteLine("kNN: {0} - real: {1}", knnResult, tuple.Value);
            }
            Console.ReadLine();
        }

        public void RunKMeans(List<CleanDataPoint> cleanData)
        {
            List<double[]> dataFields = CreateFieldsForKMeans(cleanData);

            KMeans kmeans = new KMeans(dataFields,5);

            List<KMCluster> val = kmeans.KMeanAlgorithm();

            PrintKMeansResults(val, cleanData);
        }

        private void PrintKMeansResults(List<KMCluster> val, List<CleanDataPoint> cleanData)
        {
            Console.WriteLine("Age, ProgSkill, UniYears, EngSkill");
            Console.WriteLine("##############");
            foreach (KMCluster kmCluster in val)
            {
                foreach (double[] contents in kmCluster.clusterContent)
                {
                    //Recreate Data point
                    List<int> ages = cleanData.Select(e => e.Age).ToList();
                    Console.Write(contents[0] * (ages.Max() - ages.Min()) + ages.Min());
                    Console.Write(", ");

                    List<int> progSkill = cleanData.Select(e => e.ProgrammingSkill).ToList();
                    Console.Write(contents[1] * (progSkill.Max() - progSkill.Min()) + progSkill.Min());
                    Console.Write(", ");

                    List<int> uni = cleanData.Select(e => e.UniYears).ToList();
                    Console.Write(contents[2] * (uni.Max() - 0) + 0);
                    Console.Write(", ");

                    Console.Write(contents[3] * (69- 45) + 45);
                    
                    Console.WriteLine();
                }
                Console.WriteLine("##############");
            }
            Console.ReadLine();
        }

        private List<double[]> CreateFieldsForKMeans(List<CleanDataPoint> cleanData)
        {
            List<double[]> returnFormula = new List<double[]>();

            foreach (CleanDataPoint point in cleanData)
            {
                double[] dataFields = new double[4];

                List<int> ages = cleanData.Select(e => e.Age).ToList();
                dataFields[0] = ((double)point.Age - ages.Min()) / (ages.Max() - ages.Min());

                List<int> progSkill = cleanData.Select(e => e.ProgrammingSkill).ToList();
                dataFields[1] = ((double)point.ProgrammingSkill - progSkill.Min()) / (progSkill.Max() - progSkill.Min());

                List<int> uni = cleanData.Select(e => e.UniYears).ToList();
                dataFields[2] = ((double)point.UniYears - 0) / (uni.Max() - 0);

                //EngSkill is confined within 45-69 in import
                dataFields[3] = ((double)point.EngSkill - 45) / (69 - 45);

                returnFormula.Add(dataFields);
            }

            return returnFormula;
        }

        private static Dictionary<Field[], ClassLabel>[] CreateFieldsForKNN(List<CleanDataPoint> cleanData)
        {
            Dictionary<Field[], ClassLabel>[] returnFormula = new Dictionary<Field[], ClassLabel>[2];
            returnFormula[0] = new Dictionary<Field[], ClassLabel>();
            returnFormula[1] = new Dictionary<Field[], ClassLabel>();

            foreach (CleanDataPoint point in cleanData)
            {
                int trainingOrTest = (rand.Next(0, 5) == 0 ? 1 : 0);

                Field[] dataFields = new Field[4];

                //Generating Field variables
                List<int> ages = cleanData.Select(e => e.Age).ToList();
                dataFields[0] = new NumericField(((double)point.Age - ages.Min()) / (ages.Max() - ages.Min()));

                List<int> progSkill = cleanData.Select(e => e.ProgrammingSkill).ToList();
                dataFields[1] = new NumericField(((double)point.ProgrammingSkill - progSkill.Min()) / (progSkill.Max() - progSkill.Min()));

                List<int> uni = cleanData.Select(e => e.UniYears).ToList();
                dataFields[2] = new NumericField(((double)point.UniYears - 0) / (uni.Max() - 0));

                dataFields[3] = new NumericField(((double)point.EngSkill - 45) / (69 - 45));

                //Generating ClassLabel
                ClassLabel cL = point.NN_SVM ? ClassLabel.True : ClassLabel.False;


                returnFormula[trainingOrTest].Add(dataFields, cL);
            }

            return returnFormula;
        }
    }
}
