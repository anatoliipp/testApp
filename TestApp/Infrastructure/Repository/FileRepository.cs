using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp.Infrastructure.Services;

namespace TestApp.Infrastructure.Repository;

public class FileRepository<T> : IRepository<T>
{
    //TODO: move to appsettings
    private readonly string _fileData = "Data.json"; 
    private readonly IFileService _fileService;
    public FileRepository(IFileService fileService)
    {
        _fileService = fileService;
    }

    public Task<List<T>> ListAsync()
    {
        return _fileService.RedDataFromFileAsync<List<T>>(_fileData);
    }
}