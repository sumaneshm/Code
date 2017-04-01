using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsDataStructures.Tree
{
    public class Tree<T> : TreeNode<T> where T : TreeNode<T>
    {
        public Tree() { }
    }
}
