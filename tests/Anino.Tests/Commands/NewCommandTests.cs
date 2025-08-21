using Anino.Commands;
using Anino.Services;
using FluentAssertions;
using NSubstitute;

namespace Anino.Tests.Commands;

public class NewCommandTests
{
    private readonly IAninoApplication _mockApplication;
    private readonly NewCommand _newCommand;

    public NewCommandTests()
    {
        _mockApplication = Substitute.For<IAninoApplication>();
        _newCommand = new NewCommand(_mockApplication);
    }

    [Fact]
    public void CreateCommand_ShouldReturnCommandWithCorrectName()
    {
        // Act
        var command = _newCommand.CreateCommand();

        // Assert
        command.Name.Should().Be("new");
        command.Description.Should().Be("Generate a new sample definition file with common CRUD operations");
    }

    [Fact]
    public void CreateCommand_ShouldHaveNameOption()
    {
        // Act
        var command = _newCommand.CreateCommand();

        // Assert
        var nameOption = command.Options.FirstOrDefault(o => o.Name == "--name");
        nameOption.Should().NotBeNull();
        nameOption!.Description.Should().Be("Name of the definition file to generate");
        nameOption.Aliases.Should().Contain("-n");
    }

    [Theory]
    [InlineData(null, "definition.json")]
    [InlineData("", "definition.json")]
    [InlineData("myfile", "myfile.json")]
    [InlineData("myfile.json", "myfile.json")]
    [InlineData("myfile.JSON", "myfile.JSON")]
    [InlineData("api-config", "api-config.json")]
    public void GenerateDefinitionFilename_ShouldProcessFilenamesCorrectly(string? input, string expected)
    {
        // This tests the private method logic by testing the command behavior
        // We'll use reflection to access the private method for testing
        var method = typeof(NewCommand).GetMethod("GenerateDefinitionFilename", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        
        method.Should().NotBeNull("GenerateDefinitionFilename method should exist");

        // Act
        var result = method!.Invoke(null, new object?[] { input });

        // Assert
        result.Should().Be(expected);
    }
}