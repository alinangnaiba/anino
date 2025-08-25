using System.CommandLine;
using Anino.Configuration;
using Anino.Services;

namespace Anino.Commands;

public class ScanCommand : IAninoCommand
{
    private readonly IAninoApplication _application;

    public ScanCommand(IAninoApplication application)
    {
        _application = application;
    }

    public Command CreateCommand()
    {
        var filesArgument = new Argument<string[]>("files")
        {
            Description = "C# project files (.csproj) or source files to scan for API endpoints",
            Arity = ArgumentArity.OneOrMore
        };

        var outputOption = new Option<string>("--output")
        {
            Description = "Output filename for the generated definition file",
            Aliases = { "-o" },
            DefaultValueFactory = _ => "anino-def.json"
        };

        var targetOption = new Option<string[]>("--target")
        {
            Description = "Target controller names to scan (e.g., UsersController, ProductsController). If not specified, scans all controllers.",
            Aliases = { "-t" },
            AllowMultipleArgumentsPerToken = true
        };

        var scanCommand = new Command("scan", "Scan C# project or source files to generate API definition");
        scanCommand.Arguments.Add(filesArgument);
        scanCommand.Options.Add(outputOption);
        scanCommand.Options.Add(targetOption);

        scanCommand.SetAction(parseResult =>
        {
            var files = parseResult.GetValue(filesArgument) ?? Array.Empty<string>();
            var output = parseResult.GetValue(outputOption) ?? "anino-def.json";
            var targets = parseResult.GetValue(targetOption) ?? Array.Empty<string>();

            var options = new AninoOptions
            {
                ScanFiles = files,
                ScanOutput = output,
                ScanTargets = targets
            };

            return _application.Run(options);
        });

        return scanCommand;
    }
}