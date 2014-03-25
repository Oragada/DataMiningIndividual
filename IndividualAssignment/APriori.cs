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
         
        //static int[][] TRANSACTIONS = new int[][] { { 1, 2, 3, 4, 5 }, { 1, 3, 5 }, { 2, 3, 5 }, { 1, 5 }, { 1, 3, 4 }, { 2, 3, 5 }, { 2, 3, 5 },
        //              { 3, 4, 5 }, { 4, 5 }, { 2 }, { 2, 3 }, { 2, 3, 4 }, { 3, 4, 5 } };

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

            freqSets = Sort(freqSets);
            for (int k = 1; freqSets.Count > 0; k++)
            {
                freqSets = GenerateFrequentItemSets(freqSets, data, supportThreshold, k);
                
            }

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
                        candidates.Add(kMinusPoint.Join(comp, (e => e),(e=> e),(e1,e2) => e1).ToArray());
                    }
                }
            }

            return candidates;
        }

        private static Dictionary<int[], int> GenerateFrequentItemSetsLevel1(IEnumerable<List<int>> data, int supportThreshold)
        {
            Dictionary<int[], int> freqItems = new Dictionary<int[], int>();

            //Generate the initial list
            foreach (List<int> list in data)
            {
                foreach (int i in list)
                {
                    List<int> num = new List<int>(1);
                    int[] thisNum = num.ToArray();

                    int[] exists = freqItems.Keys.First(e => e.SequenceEqual(thisNum));

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
            freqItems = (Dictionary<int[], int>) freqItems.Where(p => p.Value >= supportThreshold);

 
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
