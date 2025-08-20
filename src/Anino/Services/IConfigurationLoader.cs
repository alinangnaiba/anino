using Anino.Models;

namespace Anino.Services;

public interface IConfigurationLoader
{
    List<ApiEndpoint> LoadEndpoints(FileInfo file);
}