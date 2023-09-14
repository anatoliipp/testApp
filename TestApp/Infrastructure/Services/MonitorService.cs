using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Prism.Events;
using TestApp.Infrastructure.Events;
using TestApp.Settings;

namespace TestApp.Infrastructure.Services;

public class MonitorService : IMonitorService
{
    private readonly IFileService _fileService;
    private readonly IEventAggregator _eventAggregator;
    private Task? _timerTask;
    private readonly PeriodicTimer _timer;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly string _monitoringFile;
    private DateTime _lastChangesOfMonitoringFile = DateTime.MinValue;

    public MonitorService(IFileService fileService, IEventAggregator eventAggregator, IOptions<MonitoringServiceConfig> config)
    {
        _monitoringFile = config.Value.MonitoringFile;
        _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(config.Value.CheckIntervalInMilliseconds));
        _fileService = fileService;
        _eventAggregator = eventAggregator;
    }

    public void Start()
    {
        _timerTask = DoWorkAsync();
    }

    private async Task DoWorkAsync()
    {
        try
        {
            while (await _timer.WaitForNextTickAsync(_cancellationTokenSource.Token))
            {
                var fileInfo = _fileService.GetFileInfo(_monitoringFile);
                if (fileInfo != null && _lastChangesOfMonitoringFile != fileInfo.LastModifiedTime)
                {
                    _lastChangesOfMonitoringFile = fileInfo.LastModifiedTime;
                    _eventAggregator.GetEvent<DataChangeEvent>()
                        .Publish();
                }
            }
        }
        catch (OperationCanceledException)
        {
            //TODO: Use Logger instead CW
            Console.WriteLine("Task was canceled");
        }

        catch (Exception e)
        {
            //TODO: Use Logger instead CW
            Console.WriteLine(e);
        }
    }

    public async Task StopAsync()
    {
        if (_timerTask is null)
        {
            return;
        }

        _cancellationTokenSource.Cancel();
        await _timerTask;
        _cancellationTokenSource.Dispose();
    }
}