using Caliburn.Micro;
using RetailManager.DesktopUI.EventModels;
using System.Threading;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.ViewModels
{
	public class ShellViewModel : Conductor<Screen>, IHandle<LogOnEvent>
	{
        private readonly LoginViewModel _loginVM;
        private readonly SalesViewModel _salesVM;
        private readonly IEventAggregator _events;

        public ShellViewModel(LoginViewModel loginVM, SalesViewModel salesVM, IEventAggregator events)
        {
            _loginVM = loginVM;
            _salesVM = salesVM;
            _events = events;

            _events.SubscribeOnPublishedThread(this);

            ActivateItemAsync(loginVM);
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await DeactivateItemAsync(_loginVM, close: true, cancellationToken);
            await ActivateItemAsync(_salesVM, cancellationToken);
        }
    }
}
