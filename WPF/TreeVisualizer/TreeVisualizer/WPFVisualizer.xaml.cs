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
using System.Diagnostics;
using System.IO;

namespace TreeVisualizer
{
    /// <summary>
    /// Interaction logic for WPFVisualizer.xaml
    /// </summary>
    public partial class WPFVisualizer : UserControl
    {
        public WPFVisualizer()
        {
            InitializeComponent();
        }

        public void SetNodeToView(Node nodeVM)
        {
            theTree.ItemsSource = new Node[] {nodeVM};
            theLabel.Content = nodeVM;
        }
        //{
        //    theNodeView.DataContext = nodeVM;
        //    string fileName = @"C:\Temp\WPFLog.log";
        //    if (File.Exists(fileName))
        //    {
        //        File.Delete(fileName);
        //    }

        //    File.WriteAllText(fileName, "Children : " + nodeVM.Children.Count);
            

        
    }
}
