using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Markup;
using System.IO;


namespace LoadUncompiledXAML
{
    class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private Button Button1;

        private void InitializeComponent()
        {
            this.Width = this.Height = 500;
            using (FileStream fs = new FileStream(@"Window1.xml", FileMode.Open))
            {

                //XamlReader is used to read the XAML and compiles and returns it as DependencyObject (since we know it is DockPanel which is the root element
                // defined in the xml file, we directly typecast it as DockPanel otherwise we need to typecast it as DockPanel
                var rootElement = (DockPanel)XamlReader.Load(fs);
                this.Content = rootElement;

                //Method1
                //LogicalTreeHelper is used to browse through the tree
                //Button1 = (Button)LogicalTreeHelper.FindLogicalNode(rootElement, "button1");

                //Method2
                //Alternatively we can also use the following lines instead of the previous one
                FrameworkElement fwElem = (FrameworkElement)rootElement;
                Button1 = (Button)fwElem.FindName("button1");


                Button1.Click += new RoutedEventHandler(Button1_Click);
            }
        }

        void Button1_Click(object sender, RoutedEventArgs e)
        {
            Button1.Content = "Thank you";
        }
    }
}
