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

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for TextEditorMenu.xaml
    /// </summary>
    public partial class TextEditorMenu : UserControl
    {
        public TextEditorMenu()
        {
            InitializeComponent();
        }

        public void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Welcome to Sumanesh RichTextEdit, the most powerful editor in the world");
        }
    }
}
