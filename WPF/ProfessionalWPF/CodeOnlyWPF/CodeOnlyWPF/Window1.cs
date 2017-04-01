using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace CodeOnlyWPF
{
    class Window1 : Window
    {
        private Button Button1;

        public Window1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Width = this.Height = 400;
            this.Top = this.Left = 100;
            this.Title = "Created through code";

            DockPanel panel = new DockPanel();
            Button1 = new Button();
            Button1.Content = "Click Me";
            Button1.BorderThickness = new Thickness(30);
            Button1.Width = Button1.Height = 50;
            Button1.Click += Button1_Click;

            panel.Children.Add(Button1);
            this.AddChild(panel);
        }

        void Button1_Click(object sender, RoutedEventArgs e)
        {
            Button1.Content = "Clicked";
        }

    }
}
