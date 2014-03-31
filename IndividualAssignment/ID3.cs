using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndividualAssignment
{
    class ID3
    {
        Random rand = new Random();

        // Input:
        // Data partition, D, which is a set of training tuples and their associated class labels;
        // attribute list, the set of candidate attributes;
        // Attribute selection method, a procedure to determine the splitting criterion that “best” partitions the data tuples into individual classes. 
        //     This criterion consists of a splitting attribute and, possibly, either a split-point or splitting subset.
        
        public Node<CandidateAttribute, ClassLabel> ID3Algorithm(Dictionary<CleanDataPoint, ClassLabel> D, List<CandidateAttribute> attributeList, SelectionMethod attributeSelectionMethod  )
        {
            // (1)  create a node N;
            Node<CandidateAttribute, ClassLabel> N;
            // (2)  if tuples in D are all of the same class, C, then
            if (D.Values.All(e => e.Equals(D.Values.First())))
            {
                // (3)      return N as a leaf node labeled with the class C;
                N = new Node<CandidateAttribute, ClassLabel>(D.Values.First());
            }
            // (4)  if attribute list is empty then
            if (!attributeList.Any())
            {
                // (5)      return N as a leaf node labeled with the majority class in D; // majority voting
                List<ClassLabel> labels = D.Values.ToList();
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
                ClassLabel majorityLabel = maxLabels.Count() > 1 ? maxLabels[rand.Next(0, maxLabels.Count)] : maxLabels.First();
                N = new Node<CandidateAttribute, ClassLabel>(majorityLabel);
                //TODO
            }

            // (6)  apply Attribute selection method(D, attribute list) to find the “best” splitting criterion;
            CandidateAttribute att = attributeSelectionMethod(D, attributeList);

            // (7)  label node N with splitting criterion;
            N = new Node<CandidateAttribute, ClassLabel>(att);

            // (8)  if splitting attribute is discrete-valued and multiway splits allowed then // not restricted to binary trees
            // (9)      attribute list <= attribute list - splitting attribute; // remove splitting attribute

            // (10) for each outcome j of splitting criterion
            {
                //      // partition the tuples and grow subtrees for each partition
                // (11)     let Dj be the set of data tuples in D satisfying outcome j; // a partition
                // (12)     if Dj is empty then
                // (13)         attach a leaf labeled with the majority class in D to node N;
                // (14)     else attach the node returned by Generate decision tree(Dj , attribute list) to node N; 
                
            }
            //      endfor
            // (15) return N;
            return N;
        }
    }

    internal delegate CandidateAttribute SelectionMethod(
        Dictionary<CleanDataPoint, ClassLabel> D, List<CandidateAttribute> candList);

    public enum CandidateAttribute{ None }

    public enum ClassLabel {
        One, Two
    }

    //Based on 'NTree' implemented by Aaron Gage:
    //http://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp
    //delegate void TreeVisitor<T>(T nodeData);

    public class Node<TNode, TLeaf>
    {
        LinkedList<Node<TNode, TLeaf>> children;

        public TNode NodeData { get; set; }
        public TLeaf LeafData { get; set; }

        public Node(TNode data)
        {
            NodeData = data;
            children = new LinkedList<Node<TNode, TLeaf>>();
        }

        public Node(TLeaf data)
        {
            LeafData = data;
        }

        public void AddNode(TNode nodeData)
        {
            children.AddFirst(new Node<TNode, TLeaf>(nodeData));
        }

        public void AddLeaf(TLeaf leafData)
        {
            children.AddFirst(new Node<TNode, TLeaf>(leafData));
        }

        /*public Node<T> getChild(int i)
        {
            return children.FirstOrDefault(n => --i == 0);
        }

        public void traverse(Node<T> node, TreeVisitor<T> visitor)
        {
            visitor(node.data);
            foreach (Node<T> kid in node.children)
                traverse(kid, visitor);
        }*/
    }

    /*public class Leaf<T> : Node<T> //
    {
        public ClassLabel Attri { get; set; }

        public Leaf(T data, ClassLabel cA) : base(data)
        {
            Attri = cA;
        }
    }*/
}
