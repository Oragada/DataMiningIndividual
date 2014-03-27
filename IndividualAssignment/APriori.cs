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

        public static void RunAPriori(float supportPercentageThreshold, List<List<int>> data)
        {
            //calculate the absolute support threshold
            int supportThreshold = (int) Math.Round((supportPercentageThreshold%1)*data.Count);

            APrioriAlgorithm( data, supportThreshold );
        }

        private static void APrioriAlgorithm(List<List<int>> data, int supportThreshold)
        {


            //generate Frequent Item Sets for level 1
            Dictionary<int[], int> freqSets = GenerateFrequentItemSetsLevel1(data, supportThreshold);
            WriteItemSets(freqSets);

            freqSets = Sort(freqSets);
            for (int k = 1; freqSets.Count > 0; k++)
            {
                freqSets = GenerateFrequentItemSets(freqSets, data, supportThreshold, k);
                
            }

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
            List<int[]> candidates = Join(freqSets);
            //prune
            Dictionary<int[], int> pruned = Prune(candidates);

            return pruned;
        }

        private static Dictionary<int[], int> Prune(List<int[]> candidates)
        {
            throw new NotImplementedException();
        }

        //Working with k-1 sets
        private static List<int[]> Join(Dictionary<int[], int> freqSets)
        {
            List<int[]> kMinusOneList = new List<int[]>(freqSets.Keys);
            List<int[]> candidates = new List<int[]>();

            foreach (int[] kMinusPoint in kMinusOneList)
            {
                //int[] kMinusPoint = kMinusPoint;
                foreach (int[] comp in kMinusOneList.Where(e => !e.Equals(kMinusPoint)))
                {
                    if(kMinusPoint.Take(kMinusPoint.Length-1).SequenceEqual(comp.Take(kMinusPoint.Length-1)))
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

//    private static Hashtable<ItemSet, Integer> generateFrequentItemSets( int supportThreshold, int[][] transactions,
//                    Hashtable<ItemSet, Integer> lowerLevelItemSets ) {
//        // TODO: first generate candidate itemsets from the lower level itemsets

//        /*
//         * TODO: now check the support for all candidates and add only those
//         * that have enough support to the set
//         */

//        // TODO: return something useful
//        return null;
//    }

//    private static ItemSet joinSets( ItemSet first, ItemSet second ) {
//        // TODO: return something useful
//        return null;
//    }

//    private static Hashtable<ItemSet, Integer> generateFrequentItemSetsLevel1( int[][] transactions, int supportThreshold ) {
//        // TODO: return something useful
//        return null;
//    }

//    private static int countSupport( int[] itemSet, int[][] transactions ) {
//        // Assumes that items in ItemSets and transactions are both unique

//        // TODO: return something useful
//        return 0;
//    }

//}
//         */
    }
}
