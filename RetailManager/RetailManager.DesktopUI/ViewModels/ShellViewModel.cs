using Caliburn.Micro;
using RetailManager.DesktopUI.EventModels;
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
        private LoginViewModel _loginViewModel;
        private SalesViewModel _salesViewModel;

        public ShellViewModel(
            IEventAggregator events,
            IUserPrincipal user)
        {
            _events = events;
            _user = user;
            _events.SubscribeOnPublishedThread(this);

            _loginViewModel = IoC.Get<LoginViewModel>();
            ActivateItemAsync(_loginViewModel);
        }

        public bool IsUserLoggedIn => !string.IsNullOrWhiteSpace(_user.Token);

        public async Task ExitApplicationAsync()
        {
            await TryCloseAsync();
        }

        public async Task LogOut()
        {
            ResetUserInfo();

            await DeactivateItemAsync(_salesViewModel, close: true);

            _loginViewModel = IoC.Get<LoginViewModel>();
            await ActivateItemAsync(_loginViewModel);

            NotifyOfPropertyChange(() => IsUserLoggedIn);

            // Local function.
            void ResetUserInfo()
            {
                _user.Token = null;
                _user.EmailAddress = null;
                _user.CreatedDate = DateTime.MinValue;
                _user.FirstName = null;
                _user.LastName = null;
                _user.Id = null;
            }
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
