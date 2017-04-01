using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MarsDataStructures.Tree
{

    public class TreeNode<T> : IDisposable where T : TreeNode<T>
    {
        private T _Parent;
        public T Parent
        {
            get { return _Parent; }
            set
            {
                if (value == _Parent)
                {
                    return;
                }

                if (_Parent != null)
                {
                    _Parent.Children.Remove(this);
                }

                if (value != null && !value.Children.Contains(this))
                {
                    value.Children.Add(this);
                }

                _Parent = value;
            }
        }

        public T Root
        {
            get
            {
                //return (Parent == null) ? this : Parent.Root;

                TreeNode<T> node = this;
                while (node.Parent != null)
                {
                    node = node.Parent;
                }
                return (T)node;
            }
        }

        private TreeNodeList<T> _Children;
        public virtual TreeNodeList<T> Children
        {
            get { return _Children; }
            private set { _Children = value; }
        }

        private TreeTraversalDirection _DisposeTraversal = TreeTraversalDirection.BottomUp;
        /// <summary>
        /// Specifies the pattern for traversing the Tree for disposing of resources. Default is BottomUp.
        /// </summary>
        public TreeTraversalDirection DisposeTraversal
        {
            get { return _DisposeTraversal; }
            set { _DisposeTraversal = value; }
        }

        public TreeNode()
        {
            Parent = null;
            Children = new TreeNodeList<T>(this);
        }

        public TreeNode(T Parent)
        {
            this.Parent = Parent;
            Children = new TreeNodeList<T>(this);
        }

        public TreeNode(TreeNodeList<T> Children)
        {
            Parent = null;
            this.Children = Children;
            Children.Parent = (T)this;
        }

        public TreeNode(T Parent, TreeNodeList<T> Children)
        {
            this.Parent = Parent;
            this.Children = Children;
            Children.Parent = (T)this;
        }

        /// <summary>
        /// Reports a depth of nesting in the tree, starting at 0 for the root.
        /// </summary>
        public int Depth
        {
            get
            {
                //return (Parent == null ? -1 : Parent.Depth) + 1;

                int depth = 0;
                TreeNode<T> node = this;
                while (node.Parent != null)
                {
                    node = node.Parent;
                    depth++;
                }
                return depth;
            }
        }

        public override string ToString()
        {
            string Description = "Depth=" + Depth.ToString() + ", Children=" + Children.Count.ToString();
            if (this == Root)
            {
                Description += " (Root)";
            }
            return Description;
        }

        #region IDisposable

        private bool _IsDisposed;
        public bool IsDisposed
        {
            get { return _IsDisposed; }
        }

        public virtual void Dispose()
        {
            CheckDisposed();

            // clean up contained objects (in Value property)
            if (DisposeTraversal == TreeTraversalDirection.BottomUp)
            {
                foreach (TreeNode<T> node in Children)
                {
                    node.Dispose();
                }
            }

            OnDisposing();

            if (DisposeTraversal == TreeTraversalDirection.TopDown)
            {
                foreach (TreeNode<T> node in Children)
                {
                    node.Dispose();
                }
            }

            // TODO: clean up the tree itself

            _IsDisposed = true;
        }

        public event EventHandler Disposing;

        protected void OnDisposing()
        {
            if (Disposing != null)
            {
                Disposing(this, EventArgs.Empty);
            }
        }

        protected void CheckDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        #endregion
    }
}
