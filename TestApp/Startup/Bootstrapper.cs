using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Prism.Events;
using TestApp.Abstraction;
using TestApp.Infrastructure.Models;
using TestApp.Infrastructure.Repository;
using TestApp.Infrastructure.Services;
using TestApp.Services;
using TestApp.Settings;

namespace TestApp.Startup;

public class Bootstrapper
{
    public IContainer Bootstrap()
    {
        var builder = new ContainerBuilder();

        builder.RegisterType<MainWindow>().AsSelf();
        builder.RegisterType<MainViewModel>().AsSelf();
        builder.RegisterType<MainViewModel>().As<IBaseViewModel>();
        builder.RegisterType<MonitorService>().As<IMonitorService>();
        builder.RegisterType<FileService>().As<IFileService>();
        builder.RegisterType<FileRepository<Person>>().As<IRepository<Person>>();
        builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.json").Build();


        builder.Register(
            c => configuration
        ).As<IConfiguration>();

        builder.Register(
            c =>
            {
                var config = c.Resolve<IConfiguration>();
                var section = config.GetSection(nameof(MonitoringServiceConfig));
                var settings = new MonitoringServiceConfig();
                section.Bind(settings);
                return new OptionsWrapper<MonitoringServiceConfig>(settings);
            }).As<IOptions<MonitoringServiceConfig>>();


        return builder.Build();
    }
}