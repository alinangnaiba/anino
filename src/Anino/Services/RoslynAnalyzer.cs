using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;

namespace Anino.Services;

public class RoslynAnalyzer : IRoslynAnalyzer
{
    public SyntaxTree ParseFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var sourceCode = File.ReadAllText(filePath);
        return CSharpSyntaxTree.ParseText(sourceCode, path: filePath);
    }

    public IEnumerable<SyntaxTree> ParseFiles(IEnumerable<string> filePaths)
    {
        return filePaths.Select(ParseFile);
    }

    public async Task<(IEnumerable<SyntaxTree> SyntaxTrees, CSharpCompilation Compilation)> ParseProjectAsync(string projectPath)
    {
        try
        {
            // If it's a .csproj file, parse the entire project
            if (Path.GetExtension(projectPath).Equals(".csproj", StringComparison.OrdinalIgnoreCase))
            {
                return await ParseCSharpProjectAsync(projectPath);
            }
            
            // Otherwise, treat as individual file
            var syntaxTree = ParseFile(projectPath);
            var compilation = CSharpCompilation.Create(
                "TempAssembly",
                new[] { syntaxTree },
                GetBasicReferences(),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );
            
            return (new[] { syntaxTree }, compilation);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to parse project: {ex.Message}", ex);
        }
    }

    private async Task<(IEnumerable<SyntaxTree> SyntaxTrees, CSharpCompilation Compilation)> ParseCSharpProjectAsync(string projectPath)
    {
        var workspace = MSBuildWorkspace.Create();
        
        try
        {
            var project = await workspace.OpenProjectAsync(projectPath).ConfigureAwait(false);
            var compilation = await project.GetCompilationAsync().ConfigureAwait(false) as CSharpCompilation;
            
            if (compilation == null)
            {
                throw new InvalidOperationException("Failed to create compilation from project");
            }

            return (compilation.SyntaxTrees, compilation);
        }
        catch (Exception ex)
        {
            // If MSBuild approach fails, fall back to manual parsing
            Console.WriteLine($"MSBuild parsing failed, falling back to manual parsing: {ex.Message}");
            return ParseProjectManually(projectPath);
        }
        finally
        {
            workspace.Dispose();
        }
    }

    private (IEnumerable<SyntaxTree> SyntaxTrees, CSharpCompilation Compilation) ParseProjectManually(string projectPath)
    {
        var projectDir = Path.GetDirectoryName(projectPath)!;
        var csFiles = Directory.GetFiles(projectDir, "*.cs", SearchOption.AllDirectories)
            .Where(f => !f.Contains("bin") && !f.Contains("obj")) // Exclude build artifacts
            .ToList();

        var syntaxTrees = new List<SyntaxTree>();
        
        foreach (var file in csFiles)
        {
            try
            {
                var syntaxTree = ParseFile(file);
                syntaxTrees.Add(syntaxTree);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Failed to parse {file}: {ex.Message}");
            }
        }

        var compilation = CSharpCompilation.Create(
            Path.GetFileNameWithoutExtension(projectPath),
            syntaxTrees,
            GetBasicReferences(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        return (syntaxTrees, compilation);
    }

    private IEnumerable<MetadataReference> GetBasicReferences()
    {
        var references = new List<MetadataReference>();
        
        // Add basic .NET references
        var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location)!;
        
        references.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
        references.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll")));
        references.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Collections.dll")));
        references.Add(MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Linq.dll")));
        
        try
        {
            // Try to add ASP.NET Core references if available
            references.Add(MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Mvc.ControllerBase).Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(Microsoft.AspNetCore.Http.HttpContext).Assembly.Location));
        }
        catch
        {
            // ASP.NET Core references not available, continue without them
        }
        
        return references;
    }
}