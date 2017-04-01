using FriendStorage.UI.ViewModel;
using System.Windows;

namespace FriendStorage.UI.View
{
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            _viewModel = vm;
            DataContext = _viewModel;

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Load();
        }
    }
}
