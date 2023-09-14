using System.Threading.Tasks;
using TestApp.Services;

namespace TestApp.Infrastructure.Services;

public interface IFileService
{
    FileData? GetFileInfo(string file);
    Task<T> RedDataFromFileAsync<T>(string argsFileName) where T : class, new();
}