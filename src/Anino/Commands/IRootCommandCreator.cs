using System.CommandLine;

namespace Anino.Commands;

public interface IRootCommandCreator
{
    RootCommand Create();
}