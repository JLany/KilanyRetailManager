using Caliburn.Micro;

namespace RetailManager.DesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
		private string _username;
		private string _password;

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

                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    output = false;
                }

                return output;
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

		// This method works by convention.
		public void Login()
		{

		}
	}
}
