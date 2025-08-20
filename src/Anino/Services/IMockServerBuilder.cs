using Anino.Models;

namespace Anino.Services;

public interface IMockServerBuilder
{
    IAninoWebApplication BuildServer(List<ApiEndpoint> endpoints, int latencyMs = DefaultValueOf.Latency);
}