using System.CommandLine;
using Anino.Configuration;
using Anino.Models;
using Anino.Services;

namespace Anino.Commands;

public class ServerCommand : IAninoCommand
{
    private readonly IAninoApplication _application;

    public ServerCommand(IAninoApplication application)
    {
        _application = application;
    }

    public Command CreateCommand()
    {
        var usingOption = new Option<FileInfo>("--def")
        {
            Description = "Path to JSON configuration file",
            Required = true,
            Aliases = { "-d" }
        };

        var portOption = new Option<int>("--port")
        {
            Description = $"Server port",
            DefaultValueFactory = _ => DefaultValueOf.Port,
            Aliases = { "-p" },
        };

        var latencyOption = new Option<int>("--latency")
        {
            Description = "Simulated network latency in milliseconds (optional)",
            DefaultValueFactory = _ => DefaultValueOf.Latency,
            Aliases = { "-l" }
        };

        var startCommand = new Command("server", "Start the Anino mock API server");
        startCommand.Options.Add(usingOption);
        startCommand.Options.Add(portOption);
        startCommand.Options.Add(latencyOption);

        startCommand.SetAction(parseResult =>
        {
            var options = new AninoOptions
            {
                File = parseResult.GetValue(usingOption),
                Port = parseResult.GetValue(portOption) == 0 ? DefaultValueOf.Port : parseResult.GetValue(portOption),
                Latency = parseResult.GetValue(latencyOption),
                GenerateDefinition = null
            };

            return _application.Run(options);
        });

        return startCommand;
    }
}