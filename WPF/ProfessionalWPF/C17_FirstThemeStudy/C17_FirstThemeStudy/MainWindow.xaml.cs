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

namespace C17_FirstThemeStudy
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

        private void cboTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            lblThemeName.Content = ((ComboBoxItem)cboTheme.SelectedItem).Content;
            ResourceDictionary resDict = new ResourceDictionary();
            resDict.Source = new Uri(String.Format("Themes/{0}Theme.xaml", lblThemeName.Content),UriKind.Relative);

            //Below MergedDictionaries also works as expected
            //Application.Current.Resources.MergedDictionaries[0] = resDict;
            
            Application.Current.Resources = resDict;
        }
    }
}
