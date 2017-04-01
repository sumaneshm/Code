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

namespace MultipleCommandBindingStudy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<object, bool> isDirty;

        public MainWindow()
        {
            InitializeComponent();
            isDirty = new Dictionary<object, bool>();
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs args)
        {
            MessageBox.Show("Saving the text from " + ((TextBox)sender).Text);
            isDirty[sender] = false;
        }


        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs args)
        {
            if (isDirty.ContainsKey(sender) && isDirty[sender])
                args.CanExecute = true;
            else
                args.CanExecute = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isDirty[sender] = true;
        }
    }
}
