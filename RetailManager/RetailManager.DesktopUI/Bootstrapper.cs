using Caliburn.Micro;
using RetailManager.DesktopUI.Helpers;
using RetailManager.DesktopUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RetailManager.DesktopUI
{
	public class Bootstrapper : BootstrapperBase
	{
		private SimpleContainer _container = new SimpleContainer();

		public Bootstrapper()
		{
			Initialize();

			ConventionManager.AddElementConvention<PasswordBox>(
				PasswordBoxHelper.BoundPasswordProperty,
				"Password",
				"PasswordChanged");
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewForAsync<ShellViewModel>();
		}

		// Start configuring dependecy injection using SimpleContainer.
		// This mehtod gets called by Initialize.
		protected override void Configure()
		{
			_container.Instance(_container);

			_container
				.Singleton<IWindowManager, WindowManager>()
				.Singleton<IEventAggregator, EventAggregator>();

			_container
				.Singleton<IApiHelper, ApiHelper>();

			// Using Reflection here is OK
			// because this method is invoked only once on application startup.
			// So, no major performance hits.
			this.GetType()
				.Assembly
				.GetTypes()
				.Where(type => type.IsClass)
				.Where(type => type.Name.EndsWith("ViewModel"))
				.ToList()
				.ForEach(viewModelType
				=> _container.RegisterPerRequest(
					viewModelType, viewModelType.ToString(), viewModelType));
		}
		
		protected override object GetInstance(Type service, string key)
		{
			return _container.GetInstance(service, key);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return _container.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			_container.BuildUp(instance);
		}
	}
}
