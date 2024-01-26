using Caliburn.Micro;
using RetailManager.DesktopUI.Models;
using RetailManager.UI.Core.ApiClients;
using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RetailManager.DesktopUI.ViewModels
{
    public class UserDashboardViewModel : Screen
    {
        private readonly IUserService _userService;

        private BindingList<IdentityUserModel> _users;

        public UserDashboardViewModel(IUserService userService)
        {
            _userService = userService;
        }

        protected async override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            await base.OnActivateAsync(cancellationToken);

            try
            {
                await InitializeFormAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occurred"
                    , MessageBoxButton.OK, MessageBoxImage.Error);

                await TryCloseAsync();
            }
        }

        private async Task InitializeFormAsync()
        {
            Users = new BindingList<IdentityUserModel>((await _userService.GetUsersAsync()).ToList());
        }

        public BindingList<IdentityUserModel> Users
        {
            get { return _users; }
            set
            {
                _users = value;

                NotifyOfPropertyChange(() => Users);
            }
        }
    }
}
