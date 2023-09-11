using Caliburn.Micro;

namespace RetailManager.DesktopUI.ViewModels
{
	public class ShellViewModel : Conductor<Screen>
	{
        private readonly LoginViewModel _loginVM;

        public ShellViewModel(LoginViewModel loginVM)
        {
            _loginVM = loginVM;
            ActivateItemAsync(_loginVM);
        }

        
    }
}
