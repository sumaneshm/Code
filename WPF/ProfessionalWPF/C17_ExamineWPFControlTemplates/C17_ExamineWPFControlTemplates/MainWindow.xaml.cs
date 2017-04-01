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
using System.Reflection;
using System.Xml;
using System.Windows.Markup;

namespace C17_ExamineWPFControlTemplates
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Type controlType = typeof(Control);
            Assembly assm = controlType.Assembly;
            List<Type> derived = new List<Type>();

            foreach (var t in assm.GetTypes())
            {
                if (t.IsSubclassOf(controlType) && t.IsPublic && !t.IsAbstract)
                {
                    derived.Add(t);
                }
            }
            //derived.Sort(new TypeComparer());
            lstControls.ItemsSource = derived;
            lstControls.DisplayMemberPath = "Name";
        }

        private void lstControls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Type selectedType = (Type) lstControls.SelectedItem;

                ConstructorInfo ctrInfo = selectedType.GetConstructor(System.Type.EmptyTypes);
                Control control = (Control)ctrInfo.Invoke(null);
                control.Visibility = Visibility.Collapsed;
                
                grid.Children.Add(control);

                ControlTemplate template = control.Template;
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                StringBuilder sb = new StringBuilder();
                XmlWriter writer = XmlWriter.Create(sb, settings);
                XamlWriter.Save(template, writer);
                txtControlTemplate.Text = sb.ToString();

                grid.Children.Remove(control);

            }
            catch(Exception ex)
            {
                txtControlTemplate.Text = "Exception occurred \n\n\n" + ex.Message;
            }
        }
    }
}
