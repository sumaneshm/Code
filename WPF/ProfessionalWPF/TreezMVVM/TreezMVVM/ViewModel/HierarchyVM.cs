using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TreeLibrary;
using System.ComponentModel;
using System.Diagnostics;

namespace TreezMVVM.ViewModel
{
    [DebuggerDisplay("Root: {NodeVM}")]
    [Serializable]
    public class HierarchyVM : INotifyPropertyChanged
    {
        public override string ToString()
        {
            return String.Format("Root : {0}", Root.Name);
        }
         [Serializable]
        private class SearchCommand : ICommand
        {
            readonly HierarchyVM hierarchy;

            public SearchCommand(HierarchyVM hierarchy)
            {
                this.hierarchy = hierarchy;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            event EventHandler ICommand.CanExecuteChanged
            {
                add { }
                remove { }
            }

            public void Execute(object parameter)
            {
                hierarchy.PerformSearch();
            }
        }

        private readonly NodeVM root;
        
        private readonly ReadOnlyCollection<NodeVM> _roots;

        private SearchCommand _searchCommand;

        public NodeVM Root
        {
            get { return root; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public HierarchyVM(Node rootNode)
        {
            this.root = new NodeVM(rootNode, null);
            _roots = new ReadOnlyCollection<NodeVM>
            (new NodeVM[]
            {
                this.root
            });
            _showChorus = true;
            this._searchCommand = new SearchCommand(this);
        }

        private bool _showChorus;

        public bool ShowChorus
        {
            get { return _showChorus; }
            set
            {
                if (_showChorus != value)
                {
                    _showChorus = value;
                    ToggleNumberDisplay(_showChorus);
                }
            }
        }
      
        public ReadOnlyCollection<NodeVM> Roots
        {
            get
            {
                return _roots;
            }
        }
        public ICommand Search
        {
            get
            {
                return _searchCommand;
            }
        }

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (String.Compare(_searchText, value) != 0)
                {
                    _searchText = value;
                }
            }
        }

        private string _result;

        public string Result
        {
            get 
            { 
                return _result; 
            }
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }
        }

        public void PerformSearch()
        {
            int findId;
            NodeVM findNode;

            if (Int32.TryParse(_searchText, out findId))
            {
                findNode = Root.Find(n => (n.Number == findId));
            }
            else
            {
                findNode = Root.Find(n => n.Name.Contains(_searchText));
            }

            if (findNode != null)
            {
                findNode.IsSelected = true;
                findNode.IsExpanded = true;

                Result = "Node has been found";
            }
            else
            {
                Result = "Node with the given details could not be found";
            }
        }

        public List<NodeVM> Flatten()
        {
            return Root.FindAll(nvm => true);
        }

        public List<NodeVM> Leaves
        {
            get
            {
                return Root.FindAll(nvm=>nvm.IsLeafNode == true);
            }
        }

        public List<NodeVM> NonLeaves
        {
            get
            {
                return Root.FindAll(nvm => nvm.IsLeafNode == false);
            }
        }

        public void ToggleNumberDisplay(bool showChorus)
        {
            Root.Traverse(n => n.ToggleNumberDisplay(showChorus));
        }
    }
}
