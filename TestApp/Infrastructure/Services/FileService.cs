using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TestApp.Infrastructure.Services;

namespace TestApp.Services;

public class FileService : IFileService
{
    public FileData? GetFileInfo(string file)
    {
        try
        {
            var fileInfo = new FileInfo(file);

            return new FileData()
            {
                FileName = fileInfo.Name,
                LastModifiedTime = fileInfo.LastWriteTime

            };

        }
        catch (Exception e)
        {
            //TODO: Use Logger
        }

        return null;

    }

    public async Task<T> RedDataFromFileAsync<T>(string argsFileName) where T : class, new()
    {
        try
        {
            var fileContent = await File.ReadAllTextAsync(argsFileName);
            return JsonConvert.DeserializeObject<T>(fileContent);
        }
        catch (Exception e)
        {
            //TODO: Use Logger
        }

        return new T();

    }
}

public class FileData
{
    public string? FileName { get; set; }
    public DateTime LastModifiedTime { get; set; }
   

   
}