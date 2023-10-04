using Caliburn.Micro;
using RetailManager.DesktopUI.EventModels;
using RetailManager.UI.Core.ApiClient;
using RetailManager.UI.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RetailManager.DesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IApiHelper _apiHelper;
        private readonly IUserPrincipal _loggedInUser;
        private readonly IEventAggregator _events;
        private string _username;
		private string _password;
        private string _errorMessage;
        private bool _isLoading;

        public LoginViewModel(IApiHelper apiHelper, IUserPrincipal loggedInUser, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _loggedInUser = loggedInUser;
            _events = events;
        }

        #region UI Properties

        public string Username
		{
			get => _username;
			set
			{
				_username = value;
				NotifyOfPropertyChange(() => Username);
                NotifyOfPropertyChange(() => CanLogin);
            }
		}

		public string Password
		{
			get => _password;
			set
			{
				_password = value;
				NotifyOfPropertyChange(() => Password);
				NotifyOfPropertyChange(() => CanLogin);
			}
		}

		public bool CanLogin
		{
			get
			{
                bool output = true;

                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || IsLoading)
                {
                    output = false;
                }

                return output;
            }
		}

		public bool IsErrorVisible 
			=> !string.IsNullOrWhiteSpace(ErrorMessage);

		public string ErrorMessage
		{
			get => _errorMessage;
			set
			{
				_errorMessage = value;
				NotifyOfPropertyChange(() => ErrorMessage);
				NotifyOfPropertyChange(() => IsErrorVisible);
			}
		}


        // This method works by convention. 
        // Can<ButtonName> is wired implicitly to the button.
        // Binds parameters with arguments by convention too.
        //public bool CanLogin(string username, string password)
        //{
        //	bool output = true;

        //	if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        //	{
        //		output = false;
        //	}

        //	return output;
        //}

		public bool IsLoading
		{
			get => _isLoading;
			set
			{
				_isLoading = value;
				NotifyOfPropertyChange(() => IsLoading);
				NotifyOfPropertyChange(() => CanLogin);
			}
		}

        #endregion

        // This method works by convention.
        public async void Login()
		{
			IsLoading = true;
			ErrorMessage = "";

			try
			{
				AuthenticationModel authentication = 
					await _apiHelper.AuthenticateUserAsync(Username, Password);

				if (!authentication.IsAuthenticated)
				{
					ErrorMessage = authentication.Error_Description;
					return;
				}

				await _apiHelper.LoadLoggedInUserInfoAsync(authentication.Access_Token);

				await _events.PublishOnUIThreadAsync(new LogOnEvent());
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "An error occurred"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				IsLoading = false;
			}
		}

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
			if (close)
			{
				ClearForm();
			}

            return base.OnDeactivateAsync(close, cancellationToken);
        }

        private void ClearForm()
        {
			Username = string.Empty;
			Password = string.Empty;
        }
    }
}
