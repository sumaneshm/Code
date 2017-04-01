using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using TreeLibrary;
using System.ComponentModel;
using System.Diagnostics;

namespace TreezMVVM.ViewModel
{
    [DebuggerDisplay("Name {Name}")]
    [Serializable]
    public class NodeVM : INotifyPropertyChanged
    {

        readonly ReadOnlyCollection<NodeVM> _children;
        readonly NodeVM _parent;
        readonly Node _node;

        public NodeVM(Node node, NodeVM parent)
        {
            this._node = node;
            this._parent = parent;

            _children = new ReadOnlyCollection<NodeVM>(
                (from child in node.Children
                 select new NodeVM(child, this)).ToList<NodeVM>()
                );

            this._showChorus = true;
        }

        private bool _showChorus;

        public ReadOnlyCollection<NodeVM> Children
        {
            get { return _children; }
        }

        public string Name
        {
            get
            {
                return _node.Name;
            }
        }

        public int Number
        {
            get
            {
                if (_showChorus)
                    return _node.ChorusNodeId;
                else
                    return _node.GFRMNodeId;
            }
        }

        public int ChorusNodeId
        {
            get
            {
                return _node.ChorusNodeId;
            }
        }

        public NodeType NType
        {
            get
            {
                return _node.NType;
            }
        }

        private bool _isExpanded;

        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }
                

                if (_isExpanded && _parent != null)
                    _parent.IsExpanded = true;
            }
        }

        public bool IsLeafNode
        {
            get
            {
                return Children.Count == 0;
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ToggleNumberDisplay(bool showChorus)
        {
            this._showChorus = showChorus;

            OnPropertyChanged("Number");
        }

        public void Traverse(Action<NodeVM> nodeAction)
        {
            nodeAction(this);

            foreach (NodeVM child in this.Children)
            {
                child.Traverse(nodeAction);
            }
        }

        public List<NodeVM> FindAll(Predicate<NodeVM> nodePredicate)
        {
            List<NodeVM> leavesUnderMe = new List<NodeVM>();

            foreach (NodeVM child in this.Children)
            {
                leavesUnderMe.AddRange(child.FindAll(nodePredicate));
            }

            if (nodePredicate.Invoke(this))
            {
                leavesUnderMe.Add(this);
            }

            return leavesUnderMe;
        }

        public NodeVM Find(Predicate<NodeVM> nodePred)
        {
            if (nodePred.Invoke(this))
                return this;

            foreach (NodeVM child in this.Children)
            {
                NodeVM findNode = child.Find(nodePred);
            
                if (findNode != null)
                    return findNode;
            }

            return null;
        }

        public override string ToString()
        {
            return String.Format("{0} [{1}]", _node.ChorusNodeId, Children.Count);
        }
    }
}
