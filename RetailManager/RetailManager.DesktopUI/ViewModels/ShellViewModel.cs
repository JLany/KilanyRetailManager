using Caliburn.Micro;
using RetailManager.DesktopUI.EventModels;
using RetailManager.UI.Core.Interfaces;
using RetailManager.UI.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.ViewModels
{
	public class ShellViewModel : Conductor<Screen>, IHandle<LogOnEvent>
	{
        private readonly IEventAggregator _events;
        private readonly IUserPrincipal _user;
        private readonly IAuthenticationService _authenticationService;
        private LoginViewModel _loginViewModel;
        private SalesViewModel _salesViewModel;
        private UserDashboardViewModel _userDashboardViewModel;

        public ShellViewModel(
            IEventAggregator events,
            IUserPrincipal user,
            IAuthenticationService authenticationService)
        {
            _events = events;
            _user = user;
            _authenticationService = authenticationService;
            _events.SubscribeOnPublishedThread(this);

            _loginViewModel = IoC.Get<LoginViewModel>();
            ActivateItemAsync(_loginViewModel);
        }

        public bool IsUserLoggedIn => !string.IsNullOrWhiteSpace(_user.Token);

        public async Task ExitApplicationAsync()
        {
            await LogOut();

            await TryCloseAsync();
        }

        public async Task LogOut()
        {
            _authenticationService.EndUserSession();

            await DeactivateItemAsync(_salesViewModel, close: true);
            _salesViewModel = null;

            _loginViewModel = IoC.Get<LoginViewModel>();
            await ActivateItemAsync(_loginViewModel);

            NotifyOfPropertyChange(() => IsUserLoggedIn);
        }

        public async Task UserManagement()
        {
            _userDashboardViewModel = IoC.Get<UserDashboardViewModel>();
            await ActivateItemAsync(_userDashboardViewModel);
        }

        public async Task CashRegister()
        {
            _salesViewModel ??= IoC.Get<SalesViewModel>();

            await ActivateItemAsync(_salesViewModel);
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await DeactivateItemAsync(_loginViewModel, close: true, cancellationToken);

            _salesViewModel = IoC.Get<SalesViewModel>();
            await ActivateItemAsync(_salesViewModel, cancellationToken);

            NotifyOfPropertyChange(() => IsUserLoggedIn);
        }
    }
}
