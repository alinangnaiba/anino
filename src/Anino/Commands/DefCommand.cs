using System.CommandLine;
using Anino.Services;

namespace Anino.Commands;

public class DefCommand : IAninoCommand
{
    private readonly IAninoApplication _application;

    public DefCommand(IAninoApplication application)
    {
        _application = application;
    }

    public Command CreateCommand()
    {
        var defCommand = new Command("def", "Definition file operations");
        
        var newCommand = new NewCommand(_application);
        var scanCommand = new ScanCommand(_application);
        
        defCommand.Add(newCommand.CreateCommand());
        defCommand.Add(scanCommand.CreateCommand());

        return defCommand;
    }
}