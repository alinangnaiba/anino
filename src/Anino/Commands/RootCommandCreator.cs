using System.CommandLine;
using Anino.Services;

namespace Anino.Commands;

public class RootCommandCreator : IRootCommandCreator
{
    private readonly IAninoApplication _application;

    public RootCommandCreator(IAninoApplication application)
    {
        _application = application;
    }

    public RootCommand Create()
    {
        var rootCommand = new RootCommand("Anino - Mock API Server\n\nA lightweight tool for creating mock REST APIs from JSON configuration files.\nPerfect for frontend development, testing, and prototyping.");

        var serverCommand = new ServerCommand(_application);
        var defCommand = new DefCommand(_application);

        rootCommand.Add(serverCommand.CreateCommand());
        rootCommand.Add(defCommand.CreateCommand());

        return rootCommand;
    }
}