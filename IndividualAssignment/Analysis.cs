using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndividualAssignment
{
    public class Analysis
    {
        private static readonly Dictionary<OS, int> osInt = new Dictionary<OS, int>();
        private static readonly Dictionary<ProgrammingLanguage, int> progLangInt = new Dictionary<ProgrammingLanguage, int>();
        private static readonly Dictionary<SQL, int> sqlInt = new Dictionary<SQL, int>();


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
            Dictionary<KeyValuePair<int[], int[]>, float> results = APriori.RunAPriori(0.20f, 0.50f, data);
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

            throw new NotImplementedException();
        }
    }
}
