using System.Text.Json;
using Anino.Models;

namespace Anino.Services;

public class JsonConfigurationLoader : IConfigurationLoader
{
    public List<ApiEndpoint> LoadEndpoints(FileInfo file)
    {
        if (!file.Exists)
        {
            throw new FileNotFoundException($"Configuration file '{file.FullName}' not found.");
        }

        var jsonContent = File.ReadAllText(file.FullName);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        var endpoints = JsonSerializer.Deserialize<List<ApiEndpoint>>(jsonContent, options);
        
        if (endpoints is null || !endpoints.Any())
        {
            throw new InvalidOperationException("No endpoints found in the configuration file or the file is invalid.");
        }

        return endpoints;
    }
}