using Anino.Models;

namespace Anino.Services;

public interface IMockDataGenerator
{
    object GenerateMockResponse(DiscoveredEndpoint endpoint);
}