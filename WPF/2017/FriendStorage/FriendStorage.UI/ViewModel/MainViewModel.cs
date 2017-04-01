using System;

namespace FriendStorage.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public NavigationViewModel NavigationViewModel { get; private set; }
        public MainViewModel()
        {
            NavigationViewModel = new NavigationViewModel();
        }

        internal void Load()
        {
            NavigationViewModel.Load();
        }
    }
}
