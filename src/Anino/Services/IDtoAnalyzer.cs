using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Anino.Models;

namespace Anino.Services;

public interface IDtoAnalyzer
{
    AnalyzedTypeInfo? AnalyzeReturnType(SyntaxNode returnTypeNode, IEnumerable<SyntaxTree> syntaxTrees);
    AnalyzedTypeInfo? AnalyzeType(string typeName, IEnumerable<SyntaxTree> syntaxTrees);
    AnalyzedTypeInfo? AnalyzeReturnType(SyntaxNode returnTypeNode, CSharpCompilation compilation);
    AnalyzedTypeInfo? AnalyzeType(string typeName, CSharpCompilation compilation);
}