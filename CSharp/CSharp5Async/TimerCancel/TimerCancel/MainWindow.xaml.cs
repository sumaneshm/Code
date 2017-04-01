using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimerCancel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        

        private DateTime timeToShow;

        public DateTime TimeToShow
        {
            get
            {
                return timeToShow;
            }
            set
            {
                timeToShow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimeToShow"));
            }
        }


        public MainWindow()
        {
            TimeToShow = DateTime.Now;
            InitializeComponent();
           
        }
    }
}
