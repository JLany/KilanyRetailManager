using Caliburn.Micro;
using RetailManager.DesktopUI.ViewModels;
using System.Windows;

namespace RetailManager.DesktopUI
{
	public class Bootstrapper : BootstrapperBase
	{
		public Bootstrapper()
		{
			Initialize();
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewForAsync<ShellViewModel>();
		}
	}
}
