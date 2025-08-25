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
    private readonly IRoslynAnalyzer _roslynAnalyzer;
    private readonly IEndpointDiscoveryService _endpointDiscoveryService;
    private readonly IMockDataGenerator _mockDataGenerator;

    public AninoApplication(
        IConfigurationLoader configurationLoader,
        IMockServerBuilder serverBuilder,
        IConsoleOutput consoleOutput,
        IDefinitionGenerator templateGenerator,
        IRoslynAnalyzer roslynAnalyzer,
        IEndpointDiscoveryService endpointDiscoveryService,
        IMockDataGenerator mockDataGenerator)
    {
        _configurationLoader = configurationLoader;
        _serverBuilder = serverBuilder;
        _consoleOutput = consoleOutput;
        _templateGenerator = templateGenerator;
        _roslynAnalyzer = roslynAnalyzer;
        _endpointDiscoveryService = endpointDiscoveryService;
        _mockDataGenerator = mockDataGenerator;
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

            // Handle code scanning
            if (options.ScanFiles != null && options.ScanFiles.Any())
            {
                return HandleCodeScan(options);
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

    private int HandleCodeScan(AninoOptions options)
    {
        try
        {
            _consoleOutput.WriteInformation("Starting code scan...");

            // Validate all files exist
            foreach (var filePath in options.ScanFiles!)
            {
                if (!File.Exists(filePath))
                {
                    _consoleOutput.WriteError($"File not found: {filePath}");
                    return 1;
                }
            }

            // Determine if we have a project file or individual files
            var projectFiles = options.ScanFiles!.Where(f => f.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)).ToList();
            
            IEnumerable<DiscoveredEndpoint> discoveredEndpoints;
            
            if (projectFiles.Any())
            {
                // Use project-based compilation for better type resolution
                var projectFile = projectFiles.First();
                var (syntaxTrees, compilation) = _roslynAnalyzer.ParseProjectAsync(projectFile).Result;
                discoveredEndpoints = _endpointDiscoveryService.DiscoverEndpoints(compilation, options.ScanTargets).ToList();
            }
            else
            {
                // Fallback to individual file parsing
                var syntaxTrees = _roslynAnalyzer.ParseFiles(options.ScanFiles!);
                discoveredEndpoints = _endpointDiscoveryService.DiscoverEndpoints(syntaxTrees).ToList();
            }

            var discoveredEndpointsList = discoveredEndpoints.ToList();
            
            if (!discoveredEndpointsList.Any())
            {
                _consoleOutput.WriteWarning("No API endpoints found in the scanned files.");
                return 1;
            }

            _consoleOutput.WriteInformation($"Found {discoveredEndpointsList.Count} endpoint(s)");

            // Convert to ApiEndpoint objects with mock data
            var apiEndpoints = discoveredEndpointsList.Select(endpoint =>
            {
                var mockResponse = _mockDataGenerator.GenerateMockResponse(endpoint);
                
                return new ApiEndpoint
                {
                    Path = endpoint.Path,
                    Method = endpoint.Method,
                    StatusCode = endpoint.StatusCode,
                    Response = JsonSerializer.SerializeToElement(mockResponse)
                };
            }).ToList();

            // Serialize to JSON
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var jsonContent = JsonSerializer.Serialize(apiEndpoints, jsonOptions);

            // Write to output file
            var outputFile = options.ScanOutput ?? "anino-def.json";
            File.WriteAllText(outputFile, jsonContent);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✅ Successfully created definition file: '{outputFile}'");
            Console.ResetColor();
            Console.WriteLine("\nTo use the definition:");
            Console.WriteLine($"  anino server --def {outputFile}");

            return 0;
        }
        catch (Exception ex)
        {
            _consoleOutput.WriteError($"Error during code scan: {ex.Message}");
            return 1;
        }
    }
}