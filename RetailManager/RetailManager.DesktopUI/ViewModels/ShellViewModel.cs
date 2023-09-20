using Caliburn.Micro;

namespace RetailManager.DesktopUI.ViewModels
{
	public class ShellViewModel : Conductor<Screen>
	{
        private readonly LoginViewModel _loginVM;
        private readonly SalesViewModel _salesVM;

        public ShellViewModel(LoginViewModel loginVM, SalesViewModel salesVM)
        {
            _loginVM = loginVM;
            _salesVM = salesVM;
            ActivateItemAsync(_salesVM);
        }

        
    }
}
