namespace Anino.Services;

public class ConsoleOutput : IConsoleOutput
{
    public void WriteStartupMessage()
    {
        Console.WriteLine("--> Mapping endpoints...");
    }

    public void WriteLatencyMessage(int latencyMs)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"    ⏱️  Latency simulation enabled: {latencyMs}ms delay per request");
        Console.ResetColor();
    }

    public void WriteEndpointMapped(string httpMethod, string path)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"    ✓ Mapped [{httpMethod}] {path}");
        Console.ResetColor();
    }

    public void WriteServerRunning(int port)
    {
        Console.WriteLine($"\n🚀 Anino server is running. Listening on http://localhost:{port}");
        Console.WriteLine("Press Ctrl+C to shut down.");
    }

    public void WriteError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {message}");
        Console.ResetColor();
    }

    public void WriteInformation(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Info: {message}");
        Console.ResetColor();
    }

    public void WriteWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Warning: {message}");
        Console.ResetColor();
    }
}