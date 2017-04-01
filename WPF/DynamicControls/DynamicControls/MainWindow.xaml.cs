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

namespace DynamicControls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            

            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
            dictionary.Add("Books", new List<string> { "*", "Science", "Maths", "English", "Tamil" });
            dictionary.Add("Departments", new List<string> { "*", "Computer Science", "Bio Chemistry", "Micro biology", "History" });
            dictionary.Add("College", new List<string> { "*", "Vysya College", "Sharadha college", "AVS College", "Arts College" });
            dictionary.Add("University", new List<string> { "*", "Madras university", "Anna university", "Madhurai Kamaraj university"});
            
            grdAttributes.Children.Clear();
            
            foreach (string attributeName in dictionary.Keys)
            {
                //StackPanel stk = new StackPanel();
                grdAttributes.Children.Add(new TextBlock() { Text = attributeName, Padding=new Thickness(30,5,10,5) });
                ComboBox cbo = new ComboBox();
                foreach (string item in dictionary[attributeName])
                {
                    cbo.Items.Add(item);
                }
                cbo.SelectedIndex = 0;
                cbo.Padding = new Thickness(5);

                grdAttributes.Children.Add(cbo);
                //grdAttributes.Children.Add(stk);
            }
            


        }
    }
}
