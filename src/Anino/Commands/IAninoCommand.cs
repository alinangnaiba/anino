using System.CommandLine;

namespace Anino.Commands;

public interface IAninoCommand
{
    Command CreateCommand();
}