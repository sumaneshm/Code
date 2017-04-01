using FriendStorage.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FriendStorage.UITests.ViewModel
{
    public class NavigationViewModelTests
    {
        [Fact]
        public void ShouldLoadFriends()
        {
            NavigationViewModel viewModel = new NavigationViewModel();
            viewModel.Load();
            Assert.Equal(8, viewModel.Friends.Count);
        }
    }
}
