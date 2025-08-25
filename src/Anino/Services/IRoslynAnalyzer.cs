using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Anino.Services;

public interface IRoslynAnalyzer
{
    SyntaxTree ParseFile(string filePath);
    IEnumerable<SyntaxTree> ParseFiles(IEnumerable<string> filePaths);
    Task<(IEnumerable<SyntaxTree> SyntaxTrees, CSharpCompilation Compilation)> ParseProjectAsync(string projectPath);
}