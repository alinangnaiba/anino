using Anino.Commands;
using Anino.Services;

class Program
{
    static int Main(string[] args)
    {
        var configurationLoader = new JsonConfigurationLoader();
        var serverBuilder = new MockServerBuilder();
        var consoleOutput = new ConsoleOutput();
        var templateGenerator = new DefinitionGenerator();

        var application = new AninoApplication(configurationLoader, serverBuilder, consoleOutput, templateGenerator);
        
        var rootCommandCreator = new RootCommandCreator(application);
        var rootCommand = rootCommandCreator.Create();

        var parseResult = rootCommand.Parse(args);
        return parseResult.Invoke();
    }
}