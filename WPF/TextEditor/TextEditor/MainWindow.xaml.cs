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
using System.Printing;
using System.Windows.Forms;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DocumentManager _manager;
        public MainWindow()
        {
            InitializeComponent();
            _manager = new DocumentManager(body);
        }

        private void toolbar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox combo = (System.Windows.Controls.ComboBox)e.OriginalSource;
            if (toolbar.IsSynchronizing) return;

            if (combo == null) return;
            switch (combo.Name)
            {
                case "font":
                    _manager.ApplyToSelection(TextBlock.FontFamilyProperty,  combo.SelectedValue);
                    break;
                case "fontSize":
                    _manager.ApplyToSelection(TextBlock.FontSizeProperty, combo.SelectedValue);
                    break;
            }

            body.Focus();
        }

        private void body_SelectionChanged(object sender, RoutedEventArgs e)
        {
            toolbar.SynchronizeWith(body.Selection);
        }

        private void toolbar_ButtonClicked(object sender, RoutedEventArgs e)
        {
            status.Text = "Clicked the button" + sender;
        }

        private void NewDocument(object sender, ExecutedRoutedEventArgs e)
        {
            _manager.NewDocument();
            status.Text = "New Document";
        }

        private void OpenDocument(object sender, ExecutedRoutedEventArgs e)
        {
            _manager.OpenDocument();
            status.Text = "Opening a document";
        }

        private void SaveDocument(object sender, ExecutedRoutedEventArgs e)
        {
            _manager.SaveDocument();
            status.Text = "Save a document";
        }

        private void SaveAsDocument(object sender, ExecutedRoutedEventArgs e)
        {
            _manager.SaveAsDocument();
            status.Text = "Save As a document";
        }

        private void PrintPreview(object sender, ExecutedRoutedEventArgs e)
        {
            PrintPreviewDialog pd = new PrintPreviewDialog();
            pd.Show();
        }

        private void SaveDocument_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _manager.CanSaveDocument();
        }

    }
}
