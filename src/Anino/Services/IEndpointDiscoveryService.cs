using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Anino.Models;

namespace Anino.Services;

public interface IEndpointDiscoveryService
{
    IEnumerable<DiscoveredEndpoint> DiscoverEndpoints(IEnumerable<SyntaxTree> syntaxTrees);
    IEnumerable<DiscoveredEndpoint> DiscoverEndpoints(CSharpCompilation compilation);
    IEnumerable<DiscoveredEndpoint> DiscoverEndpoints(CSharpCompilation compilation, IEnumerable<string>? targetControllers);
}