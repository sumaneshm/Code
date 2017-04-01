// This collection of non-binary tree data structures created by Dan Vanderboom.
// Critical Development blog: http://dvanderboom.wordpress.com
// Original Tree<T> blog article: http://dvanderboom.wordpress.com/2008/03/15/treet-implementing-a-non-binary-tree-in-c/

using System;
using System.Text;

namespace System.Collections.Generic
{
    /// <summary>
    /// Contains a list of ComplexTreeNode (or ComplexTreeNode-derived) objects, with the capability of linking parents and children in both directions.
    /// </summary>
    public class ComplexTreeNodeList<T> : List<ComplexTreeNode<T>> where T : ComplexTreeNode<T>
    {
        public T Parent;

        public ComplexTreeNodeList(ComplexTreeNode<T> Parent)
        {
            this.Parent = (T)Parent;
        }

        public T Add(T Node)
        {
            base.Add(Node);
            Node.Parent = Parent;
            return Node;
        }

        public override string ToString()
        {
            return "Count=" + Count.ToString();
        }
    }
}