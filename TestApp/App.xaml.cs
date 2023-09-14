using System.Windows;
using Autofac;
using TestApp.Infrastructure.Services;
using TestApp.Startup;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IMonitorService _monitorService;
        
        protected override void OnStartup(StartupEventArgs e)
        {
          
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();
            
  
            _monitorService = container.Resolve<IMonitorService>();
            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
            _monitorService.Start();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _monitorService.StopAsync();
            base.OnExit(e);
        }
    }
}