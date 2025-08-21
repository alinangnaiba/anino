using System.Text.Json;
using Anino.Configuration;
using Anino.Models;

namespace Anino.Services;

public class AninoApplication : IAninoApplication
{
    private readonly IConfigurationLoader _configurationLoader;
    private readonly IMockServerBuilder _serverBuilder;
    private readonly IConsoleOutput _consoleOutput;
    private readonly IDefinitionGenerator _templateGenerator;

    public AninoApplication(
        IConfigurationLoader configurationLoader,
        IMockServerBuilder serverBuilder,
        IConsoleOutput consoleOutput,
        IDefinitionGenerator templateGenerator)
    {
        _configurationLoader = configurationLoader;
        _serverBuilder = serverBuilder;
        _consoleOutput = consoleOutput;
        _templateGenerator = templateGenerator;
    }

    public int Run(AninoOptions options)
    {
        try
        {
            // Handle template generation
            if (!string.IsNullOrEmpty(options.GenerateDefinition))
            {
                _templateGenerator.GenerateDefinition(options.GenerateDefinition);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✓ Template generated successfully at '{options.GenerateDefinition}'");
                Console.ResetColor();
                Console.WriteLine("\nTo use the definition:");
                Console.WriteLine($"  anino server --def {options.GenerateDefinition}");
                return 0;
            }

            if (options.File == null)
            {
                _consoleOutput.WriteError("--def parameter is required.");
                Console.WriteLine("Usage: anino server --def <path-to-json-def> [--port <port>] [--latency <ms>]");
                Console.WriteLine("   or: anino def new --name <filename>");
                return 1;
            }

            var endpoints = _configurationLoader.LoadEndpoints(options.File);
            var app = _serverBuilder.BuildServer(endpoints, options.Latency);

            _consoleOutput.WriteStartupMessage();
            
            if (options.Latency > DefaultValueOf.Latency)
            {
                _consoleOutput.WriteLatencyMessage(options.Latency);
            }

            foreach (var endpoint in endpoints)
            {
                var httpMethod = endpoint.Method.ToUpperInvariant();
                _consoleOutput.WriteEndpointMapped(httpMethod, endpoint.Path);
            }

            _consoleOutput.WriteServerRunning(options.Port);
            app.Run($"http://localhost:{options.Port}");

            return 0;
        }
        catch (FileNotFoundException)
        {
            _consoleOutput.WriteError($"File '{options.File?.FullName}' not found.");
            return 1;
        }
        catch (JsonException ex)
        {
            _consoleOutput.WriteError($"Invalid JSON format in file '{options.File?.FullName}': {ex.Message}");
            return 1;
        }
        catch (InvalidOperationException ex)
        {
            _consoleOutput.WriteError(ex.Message);
            return 1;
        }
        catch (Exception ex)
        {
            _consoleOutput.WriteError(ex.Message);
            return 1;
        }
    }
}