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
using System.Collections.ObjectModel;

namespace TreezVirtualizing
{
    public class TreeViewData : ObservableCollection<ItemsForTreeView>
    {

        public TreeViewData()
        {
            for (int i = 0; i < 100; ++i)
            {
                ItemsForTreeView item = new ItemsForTreeView();
                item.TopLevelName = "item " + i.ToString();
                Add(item);
            }
        }
    }


    public class ItemsForTreeView
    {
        public string TopLevelName { get; set; }
        private ObservableCollection<string> level2Items;

        public ObservableCollection<string> SecondLevelItems
        {
            get
            {
                if (level2Items == null)
                {
                    level2Items = new ObservableCollection<string>();
                }
                return level2Items;
            }
        }

        public ItemsForTreeView()
        {
            for (int i = 0; i < 10; ++i)
            {
                SecondLevelItems.Add("Second Level " + i.ToString());
            }
        }
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
