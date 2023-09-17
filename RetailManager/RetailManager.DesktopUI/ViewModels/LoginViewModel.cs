using Caliburn.Micro;
using RetailManager.UI.Core.ApiClient;
using RetailManager.UI.Core.Models;
using System;
using System.Windows;

namespace RetailManager.DesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IApiHelper _apiHelper;
        private string _username;
		private string _password;
        private string _errorMessage;

        public LoginViewModel(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

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

        private bool _isLoading;

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

        // This method works by convention.
        public async void Login()
		{
			IsLoading = true;

			try
			{
				AuthenticationModel authentication = 
					await _apiHelper.AuthenticateUserAsync(Username, Password);

				if (!authentication.IsAuthenticated)
				{
					ErrorMessage = authentication.Error_Description;
				}
				else
					ErrorMessage = string.Empty;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "An error occurred"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
			}

			IsLoading = false;
		}
	}
}
