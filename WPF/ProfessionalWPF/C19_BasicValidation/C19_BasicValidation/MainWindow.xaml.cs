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

namespace C19_BasicValidation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            lstProducts.ItemsSource = App.StoreDB.Products;

        }

        private void btnFetch_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Product prod = (Product)gridProductDetails.DataContext;

            App.StoreDB.UpdateProduct(prod);

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Product prod = (Product)gridProductDetails.DataContext;

            App.StoreDB.DeleteProduct(prod);
        }

        private void gridProductDetails_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                lblResult.Content = e.Error.ErrorContent.ToString();
            }
            else
            {
                lblResult.Content = "";
            }
        }
    }
}
