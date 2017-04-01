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

namespace ModelInheritence
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new DerivedClass {Age = 30, Name = "Sumanesh"};

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           // BaseClass baser = new DerivedClass {Age = 2, Name = "Aadhavan"};
            BaseClass baser = new StudentDerivedClass { RollNumber=  2, Name = "Aadhavan" };
            this.DataContext = baser;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BaseClass baser = new DerivedClass { Age = 28, Name = "Saveetha" };
            this.DataContext = baser;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            BaseClass baser = new BaseClass{ Name = "Saveetha" };
            this.DataContext = baser;
        }
    }
}
