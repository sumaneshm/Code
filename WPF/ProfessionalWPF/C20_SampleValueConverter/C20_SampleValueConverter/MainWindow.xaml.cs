using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace C20_SampleValueConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<MyDataItem> dataItems;
        public ObservableCollection<MyDataItem> DataItems
        {
            get { return dataItems; }
        }

        
        public MainWindow()
        {
            dataItems = new ObservableCollection<MyDataItem>();
            dataItems.Add(new MyDataItem("Sumanesh", DType.STUDENT));
            dataItems.Add(new MyDataItem("Saveetha", DType.STUDENT));
            dataItems.Add(new MyDataItem("Aadhavan", DType.TEACHER));
            dataItems.Add(new MyDataItem("Nila", DType.TEACHER));


            //dataItems.Add(new MyDataItem("Sumanesh","STUDENT"));
            //dataItems.Add(new MyDataItem("Saveetha", "STUDENT"));
            //dataItems.Add(new MyDataItem("Aadhavan", "TEACHER"));
            //dataItems.Add(new MyDataItem("Nila", "TEACHER"));

            InitializeComponent();

        }
    }
}
