using System.CommandLine;
using Anino.Configuration;
using Anino.Models;
using Anino.Services;

namespace Anino.Commands;

public class NewCommand : IAninoCommand
{
    private readonly IAninoApplication _application;

    public NewCommand(IAninoApplication application)
    {
        _application = application;
    }

    public Command CreateCommand()
    {
        var nameOption = new Option<string>("--name")
        {
            Description = $"Name of the definition file to generate",
            DefaultValueFactory = _ => DefaultValueOf.DefinitionFilename,
            Aliases = { "-n" }
        };

        var newCommand = new Command("new", "Generate a new sample definition file with common CRUD operations");
        newCommand.Options.Add(nameOption);

        newCommand.SetAction(parseResult =>
        {
            var templateName = parseResult.GetValue(nameOption);
            var processedTemplateName = GenerateDefinitionFilename(templateName);

            var options = new AninoOptions
            {
                File = null,
                Port = DefaultValueOf.Port,
                Latency = 0,
                GenerateDefinition = processedTemplateName
            };

            return _application.Run(options);
        });

        return newCommand;
    }

    private static string? GenerateDefinitionFilename(string? filename)
    {
        if (string.IsNullOrEmpty(filename))
        {
            return DefaultValueOf.DefinitionFilename;
        }

        if (!filename.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
        {
            return filename + ".json";
        }

        return filename;
    }
}