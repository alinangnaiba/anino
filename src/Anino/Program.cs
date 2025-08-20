using System.CommandLine;
using Anino.Configuration;
using Anino.Models;
using Anino.Services;

class Program
{
    static int Main(string[] args)
    {
        var fileOption = new Option<FileInfo>("--file")
        {
            Description = "Path to JSON configuration file"
        };

        var portOption = new Option<int>("--port")
        {
            Description = $"Server port (default: {DefaultValueOf.Port})"
        };

        var latencyOption = new Option<int>("--latency")
        {
            Description = "Simulated network latency in milliseconds (optional)"
        };

        var generateTemplateOption = new Option<string?>("--generate-template")
        {
            Description = "Generate a sample template file with common CRUD operations (default: template.json)",
            Arity = ArgumentArity.ZeroOrOne
        };

        var rootCommand = new RootCommand("Anino - Mock API Server\n\nA lightweight tool for creating mock REST APIs from JSON configuration files.\nPerfect for frontend development, testing, and prototyping.");
        rootCommand.Options.Add(fileOption);
        rootCommand.Options.Add(portOption);
        rootCommand.Options.Add(latencyOption);
        rootCommand.Options.Add(generateTemplateOption);

        rootCommand.SetAction(parseResult =>
        {
            var generateTemplateValue = parseResult.GetValue(generateTemplateOption);
            var isGenerateTemplateProvided = parseResult.CommandResult.GetResult(generateTemplateOption) != null;
            var processedGenerateTemplate = ProcessGenerateTemplateFilename(generateTemplateValue, isGenerateTemplateProvided);

            var options = new AninoOptions
            {
                File = parseResult.GetValue(fileOption),
                Port = parseResult.GetValue(portOption) == 0 ? DefaultValueOf.Port : parseResult.GetValue(portOption),
                Latency = parseResult.GetValue(latencyOption),
                GenerateTemplate = processedGenerateTemplate
            };

            var configurationLoader = new JsonConfigurationLoader();
            var serverBuilder = new MockServerBuilder();
            var consoleOutput = new ConsoleOutput();
            var templateGenerator = new TemplateGenerator();
            var application = new AninoApplication(configurationLoader, serverBuilder, consoleOutput, templateGenerator);

            return application.Run(options);
        });

        var parseResult = rootCommand.Parse(args);
        return parseResult.Invoke();
    }

    private static string? ProcessGenerateTemplateFilename(string? filename, bool optionProvided)
    {
        if (!optionProvided)
        {
            return null;
        }

        // If option is provided but no filename (e.g., just --generate-template), use default
        if (string.IsNullOrEmpty(filename))
        {
            return "template.json";
        }

        // If filename doesn't end with .json, append it
        if (!filename.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
        {
            return filename + ".json";
        }

        // Use the filename as provided
        return filename;
    }
}