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

namespace DataGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Person> myList;
        public MainWindow()
        {
            InitializeComponent();
            myList = new List<Person>();
            myList.Add(new Person() { name = "john", age = 25, member = true });
            myList.Add(new Person() { name = "jill", age = 25, member = false });
            myList.Add(new Person() { name = "bill", age = 15, member = true });
            dataGrid1.ItemsSource = myList;

            _attributes = new ObservableCollection<Pair>();
            _attributes.Add(new Pair("LE","5"));

            DataContext = this;
        }

        public class Pair
        {
            public string Name { set; get; }
            public string Value { set; get; }

            public Pair()
            {
            }

            public Pair(string name, string value)
            {
                this.Name = name;
                this.Value = value;
            }
        }
        public class Person
        {
            public string name { set; get; }
            public int age { set; get; }
            public bool member { set; get; }

            public Person()
            {
            }
        }


        private List<String> TestBinding
        {
            get
            {
                return new List<String>() { "Sumanesh", "Saveetha", "Aadhavan" };
            }
        }
        private ObservableCollection<Pair> _attributes;
        public ObservableCollection<Pair> Attributes
        {
            get
            {
                return _attributes;
            }
            set
            {
                _attributes = value;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(myList.Count.ToString());
        }
    }
}
