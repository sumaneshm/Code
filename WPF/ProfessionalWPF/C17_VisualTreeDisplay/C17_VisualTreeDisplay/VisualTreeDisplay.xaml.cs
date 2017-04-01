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
using System.Windows.Shapes;

namespace C17_VisualTreeDisplay
{
    /// <summary>
    /// Interaction logic for VisualTreeDisplay.xaml
    /// </summary>
    public partial class VisualTreeDisplay : Window
    {
        public VisualTreeDisplay()
        {
            InitializeComponent();
        }

        DependencyObject prev;
        Brush prevBrush;

        public void ShowElements(DependencyObject element)
        {
            treeElements.Items.Clear();
            ProcessElement(element, null);
        }

        public void ProcessElement(DependencyObject element, TreeViewItem previousItem)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = element.GetType().Name;
            item.IsExpanded = true;
            item.Tag = element;

            if (previousItem == null)
            {
                treeElements.Items.Add(item);
            }
            else
            {
                previousItem.Items.Add(item);
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                ProcessElement(VisualTreeHelper.GetChild(element, i), item);
            }
        }

        private void treeElements_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (prev != null)
                prev.SetValue(BackgroundProperty, prevBrush);

            DependencyObject obj = (DependencyObject)((TreeViewItem)((TreeView)sender).SelectedItem).Tag;
            prev = obj;
            prevBrush = (Brush) prev.GetValue(BackgroundProperty);


            obj.SetValue(BackgroundProperty, new SolidColorBrush(Colors.Red));
        }
    }
}
