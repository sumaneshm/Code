using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TreezMVVM.ViewModel;
using TreeLibrary;

namespace TreezMVVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly HierarchyVM hierarchy;
 
        public MainWindow()
        {
            InitializeComponent();

            Node root = NodeTreeGenerator.GenerateTree();

            hierarchy = new HierarchyVM(root);
            //this.DataContext = hierarchy;
            theHierView.DataContext = hierarchy;
        }
    }
}
