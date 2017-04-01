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
using System.ComponentModel;

namespace DependencyPropertyStudy
{
    /// <summary>
    /// Interaction logic for Student.xaml
    /// </summary>
    public partial class Student : UserControl, INotifyPropertyChanged
    {
        public Student()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        FrameworkPropertyMetadata md = new FrameworkPropertyMetadata();
        

        // Using a DependencyProperty as the backing store for CaptionProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(Student), new PropertyMetadata("DEFAULTING1", OnCaptionPropertyChanged), 
            new ValidateValueCallback(
                (object value)=>
                {
                    return value.ToString().Length>10;
                }
                ));

        

        private static void OnCaptionPropertyChanged(DependencyObject dependencyObject,
               DependencyPropertyChangedEventArgs e)
        {
            Student myUserControl = dependencyObject as Student;
           // myUserControl.OnPropertyChanged("Caption");
            myUserControl.OnCaptionPropertyChanged(e);
        }
        private void OnCaptionPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            txtCaption.Text = Caption; 
        }
    }
}
