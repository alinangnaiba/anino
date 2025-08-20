using Anino.Models;

namespace Anino.Services;

public interface IConsoleOutput
{
    void WriteStartupMessage();
    void WriteLatencyMessage(int latencyMs);
    void WriteEndpointMapped(string httpMethod, string path);
    void WriteServerRunning(int port);
    void WriteError(string message);
}