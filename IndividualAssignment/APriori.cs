using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace IndividualAssignment
{
    class APriori
    {
        
        //The TRANSACTIONS 2-dimensional array holds the full data set for the lab

        public static List<List<int>> TRANSACTIONS = new List<List<int>>() { new List<int>(){ 1, 2, 3, 4, 5 }, new List<int>(){ 1, 3, 5 }, new List<int>(){ 2, 3, 5 }, 
                        new List<int>(){ 1, 5 }, new List<int>(){ 1, 3, 4 }, new List<int>(){ 2, 3, 5 }, new List<int>(){ 2, 3, 5 }, new List<int>(){ 3, 4, 5 }, 
                        new List<int>(){ 4, 5 }, new List<int>(){ 2 }, new List<int>(){ 2, 3 }, new List<int>(){ 2, 3, 4 }, new List<int>(){ 3, 4, 5 } };

        public static Dictionary<KeyValuePair<int[], int[]>, float> RunAPriori(float supportPercentageThreshold, float confidencePercentageThreshold, List<List<int>> data)
        {
            //calculate the absolute support threshold
            int supportThreshold = (int) Math.Round((supportPercentageThreshold%1)*data.Count);
            //int confidenceThreshold = (int) Math.Round((confidencePercentageThreshold%1)*data.Count);

            return APrioriAlgorithm( data, supportThreshold, confidencePercentageThreshold );
        }

        private static Dictionary<KeyValuePair<int[], int[]>, float> APrioriAlgorithm(List<List<int>> data, int supportThreshold, float confidenceThreshold)
        {
            //generate Frequent Item Sets for level 1
            Dictionary<int[], int> freqSets = GenerateFrequentItemSetsLevel1(data, supportThreshold);
            //WriteItemSets(freqSets);

            freqSets = Sort(freqSets);
            for (int k = 2; freqSets.Count > 0; k++)
            {
                Dictionary<int[], int> newFreqSets = GenerateFrequentItemSets(freqSets, data, supportThreshold, k);
                freqSets = freqSets.Union(newFreqSets).ToDictionary((e => e.Key), e => e.Value);
                if (newFreqSets.Count <= k)
                {
                    break;
                }
            }

            //create association rules
            //List of ((int[] => int[]), confidence)
            Dictionary<KeyValuePair<int[], int[]>, float> AssocRules = new Dictionary<KeyValuePair<int[], int[]>, float>();
            //For each frequent itemset l, generate all nonempty subsets of l.
            foreach (int[] itemset in freqSets.Keys.Where(e => e.Length > 1))
            {
                float confIS = freqSets[itemset];
                List<int[]> subsets = GenerateSubsets(itemset, freqSets);
                foreach (int[] subset in subsets)
                {
                    float confidenceVal = (confIS / ((float)freqSets[subset]));
                    if (confidenceVal >= confidenceThreshold)
                    {
                        //WriteAssoRule(subset, itemset, confidenceVal);
                        AssocRules.Add(new KeyValuePair<int[], int[]>(subset, itemset.Where(e => !subset.Contains(e)).ToArray()), confidenceVal);
                    }
                }
            }
            //Console.ReadLine();

            return AssocRules;
            //return something useful
        }

        private static List<int[]> GenerateSubsets(int[] itemset, Dictionary<int[], int> freqSets)
        {
            //Since all subsets must be contained in frequent sets (APriori property), these can be found in the frequent set list
            //Simply returns all frequent itemsets where all elements are contained in the given itemset
            return freqSets.Keys.Where(e => !e.Equals(itemset)).Where(e => e.All(itemset.Contains)).ToList();
        }

        private static void WriteAssoRule(int[] s, int[] l, float confidenceVal)
        {
            int[] lMinusS = l.Where(e => !s.Contains(e)).ToArray();
            Console.WriteLine("[{0}] => [{1}], Cofidence: {2}%",PrintArray(s), PrintArray(lMinusS), confidenceVal*100);
        }

        private static string PrintArray(int[] arr)
        {
            StringBuilder strB = new StringBuilder();
            foreach (int i in arr)
            {
                if (strB.Length != 0)
                {
                    strB.Append(", ");
                }
                strB.Append(i);
            }
            return strB.ToString();
        }

        private static void WriteItemSets(Dictionary<int[], int> freqSets)
        {
            foreach (int[] itemSet in freqSets.Keys)
            {
                Console.Write("Set: [");
                foreach (int i in itemSet)
                {
                    Console.Write(i + ", ");
                }
                Console.WriteLine("] Count: {0}", freqSets[itemSet]);
            }
            //freqSets.Keys.ToList().ForEach(e => Console.WriteLine(string.Format("Set {1}, count {0}", freqSets[e], e.ToString())));
        }

        private static Dictionary<int[], int> Sort(Dictionary<int[], int> freqSets)
        {
            List<KeyValuePair<int[], int>> freqSorted = freqSets.OrderBy(e => e.Key[0]).ToList();
            Dictionary<int[], int> freqSortedDict = new Dictionary<int[], int>();
            freqSorted.ForEach(e => freqSortedDict.Add(e.Key, e.Value));

            return freqSortedDict;
        }

        private static Dictionary<int[], int> GenerateFrequentItemSets(Dictionary<int[], int> freqSets, List<List<int>> data, int supportThreshold, int k)
        {
            //join
            List<int[]> candidates = Join(freqSets, k);
            //prune
            Dictionary<int[], int> pruned = Prune(candidates, freqSets, supportThreshold, data);

            return pruned;
        }

        //Works for k1 sets
        private static Dictionary<int[], int> Prune(List<int[]> candidates, Dictionary<int[], int> freqSets, int supportThreshold, List<List<int>> data)
        {
            Dictionary<int[], int> newFreqSets = new Dictionary<int[], int>();
            foreach (int[] candidate in candidates)
            {
                //We check that the candidate minus each of its elements exists in freqSets
                bool exists = candidate.Aggregate(true, (current, i) => current & freqSets.Keys.Any(e => e.SequenceEqual(candidate.Except(new[] {i}))));
                if (exists)
                {
                    //Check that the candidate is supported
                    int count = CountOccurance(candidate, data);
                    if (count >= supportThreshold)
                    {
                        newFreqSets.Add(candidate, count);
                    }
                }
            }
            return newFreqSets;
        }

        private static int CountOccurance(int[] candidate, List<List<int>> data)
        {
            return data.Count(list => candidate.All(list.Contains));
        }

        //Works with k1 sets
        private static List<int[]> Join(Dictionary<int[], int> freqSets, int k)
        {
            List<int[]> kMinusOneList = new List<int[]>(freqSets.Keys.Where(e => e.Length == k-1));
            //if(k == 1) kMinusOneList = new List<int[]>(freqSets.Keys);
            List<int[]> candidates = new List<int[]>();

            foreach (int[] kMinusPoint in kMinusOneList)
            {
                //int[] kMinusPoint = kMinusPoint;
                foreach (int[] comp in kMinusOneList.Where(e => !e.Equals(kMinusPoint)))
                {
                    if(kMinusPoint.Take(k-2).SequenceEqual(comp.Take(k-2)) || k == 2)
                    {
                        if(comp.Last() > kMinusPoint.Last()) candidates.Add(kMinusPoint.Union(comp).ToArray());
                    }
                }
            }

            return candidates;
        }

        //working
        private static Dictionary<int[], int> GenerateFrequentItemSetsLevel1(IEnumerable<List<int>> data, int supportThreshold)
        {
            Dictionary<int[], int> freqItems = new Dictionary<int[], int>();

            //Generate the initial list
            foreach (List<int> list in data)
            {
                foreach (int i in list)
                {

                    int[] thisNum = new[]{i};

                    int[] exists = freqItems.Keys.FirstOrDefault(e => e.SequenceEqual(thisNum));

                    if (exists != null)
                    {
                        freqItems[exists]++;
                    }
                    else
                    {
                        freqItems.Add(thisNum, 1);
                    }
                }
            }
            //Exclude any values below the support threshold
            freqItems = freqItems.Where(p => p.Value >= supportThreshold).ToDictionary((e => e.Key),(e => e.Value));
            //freqItems = (Dictionary<int[], int>) 

 
            return freqItems;
        }


//    public static List<ItemSet> apriori( int[][] transactions, int supportThreshold ) {
//        int k;
//        Hashtable<ItemSet, Integer> frequentItemSets = generateFrequentItemSetsLevel1( transactions, supportThreshold );
//        for (k = 1; frequentItemSets.size() > 0; k++) {
//            System.out.print( "Finding frequent itemsets of length " + (k + 1) + "…" );
//            frequentItemSets = generateFrequentItemSets( supportThreshold, transactions, frequentItemSets );
//            // TODO: add to list

//            System.out.println( " found " + frequentItemSets.size() );
//        }
//        // TODO: create association rules from the frequent itemsets

//        // TODO: return something useful
//        return null;
//    }
    }
}
