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

namespace RoutedEventsStudy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            lblClickMe.AddHandler(UIElement.MouseUpEvent, new MouseButtonEventHandler( obj_MouseUp));
            lblClickMe.AddHandler(UIElement.KeyUpEvent, new RoutedEventHandler(obj_MouseUp));
            lblClickMe.AddHandler(UIElement.PreviewMouseUpEvent, new RoutedEventHandler(obj_MouseUp));
            txtName.AddHandler(UIElement.PreviewKeyDownEvent, new RoutedEventHandler(obj_MouseUp));
        }
        protected int eventCount = 0;

        private void obj_MouseUp(object sender, RoutedEventArgs e)
        {
            eventCount++;
            lstMessages.Items.Add(string.Format("#{0}:\nSender: {1}\nSource: {2}\nOriginal Sender: {3}\nEvent : {4}", eventCount,sender,e.Source,e.OriginalSource, e.RoutedEvent.Name));
            e.Handled = (bool)chkIsHandled.IsChecked;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lstMessages.Items.Clear();
            eventCount = 0;
        }
    }
}
