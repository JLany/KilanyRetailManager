using Caliburn.Micro;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailManager.UI.Core.Configuration;
using RetailManager.DesktopUI.Extensions;
using RetailManager.DesktopUI.Helpers;
using RetailManager.DesktopUI.ViewModels;
using RetailManager.UI.Core.Extensions;
using Serilog;
using System.Windows;
using System.Windows.Controls;

namespace RetailManager.DesktopUI
{
    public class Bootstrapper : BootstrapperBase
    {
        //private SimpleContainer _container = new SimpleContainer();

        private IHost _host;

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
            var builder = Infrastructure.InitConfiguration();
            Infrastructure.ConfigureLogger(builder);

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((HostBuilderContext context, IServiceCollection services) =>
                {
                    services
                        .AddMVVM(this)
                        .AddRetailManagerUiCore()
                        .ConfigureAutoMapper(); 
                })
                .UseSerilog()
                .Build();

            //_container.Instance(_container);

            //// WPF Services.
            //_container
            //	.Singleton<IWindowManager, WindowManager>()
            //	.Singleton<IEventAggregator, EventAggregator>();

            //_container.ConfigureAutoMapper();

            //// RetailManager.UI.Core Services.
            //_container
            //	.AddRetailManagerUiCore();

            //// Using Reflection here is OK
            //// because this method is invoked only once on application startup.
            //// So, no major performance hits.
            //this.GetType()
            //	.Assembly
            //	.GetTypes()
            //	.Where(type => type.IsClass)
            //	.Where(type => type.Name.EndsWith("ViewModel"))
            //	.ToList()
            //	.ForEach(viewModelType
            //	=> _container.RegisterPerRequest(
            //		viewModelType, viewModelType.ToString(), viewModelType));
        }

        protected override object GetInstance(Type service, string key)
        {
            //return _container.GetInstance(service, key);
            return _host.Services.GetService(service)!;
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            //return _container.GetAllInstances(service);
            return _host.Services.GetServices(service)!;
        }

        //protected override void BuildUp(object instance)
        //{
        //	_container.BuildUp(instance);
        //}
    }
}
