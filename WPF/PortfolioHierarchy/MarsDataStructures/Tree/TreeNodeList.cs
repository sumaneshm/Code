using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsDataStructures.Tree
{

    public class TreeNodeList<T> : List<TreeNode<T>> where T : TreeNode<T>
    {
        public T Parent;

        public TreeNodeList(TreeNode<T> Parent)
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
