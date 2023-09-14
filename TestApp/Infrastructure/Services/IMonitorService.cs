using System.Threading.Tasks;

namespace TestApp.Infrastructure.Services;

public interface IMonitorService
{
    void Start();
    Task StopAsync();
}