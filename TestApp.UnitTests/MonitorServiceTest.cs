using Microsoft.Extensions.Options;
using NSubstitute;
using Prism.Events;
using TestApp.Infrastructure.Events;
using TestApp.Infrastructure.Services;
using TestApp.Services;
using TestApp.Settings;

namespace TastApp.UnitTests;

public class MonitorServiceTest
{
    private readonly MonitorService _monitorService;
    private readonly IFileService _fileService;
    private readonly IEventAggregator _eventAggregator;
    private readonly IOptions<MonitoringServiceConfig> _config;

    public MonitorServiceTest()
    {
        _fileService = Substitute.For<IFileService>();
        _eventAggregator = Substitute.For<IEventAggregator>();
        _config = Substitute.For<IOptions<MonitoringServiceConfig>>();
        _config.Value.Returns(
            new MonitoringServiceConfig()
            {
                CheckIntervalInMilliseconds = 1,
                MonitoringFile = "Data.json"
            });

        _monitorService = new MonitorService(_fileService, _eventAggregator, _config);
    }

    [Fact]
    public async void SendNotifyOnlyWhenFileChanged()
    {
        //Arrange
        _fileService.GetFileInfo(Arg.Any<string>())
            .Returns(
                i => new FileData()
                {
                    FileName = "FileName",
                    LastModifiedTime = DateTime.MinValue
                });

        //Act
        _monitorService.Start();

        //Assert
        _eventAggregator.DidNotReceive().GetEvent<DataChangeEvent>();

        //Arrange
        _fileService.GetFileInfo(Arg.Any<string>())
            .Returns(
                i => new FileData()
                {
                    FileName = "FileName",
                    LastModifiedTime = DateTime.Now
                });

        await Task.Delay(2);

        //Assert
        _eventAggregator.Received(1).GetEvent<DataChangeEvent>();
    }
}