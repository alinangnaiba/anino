using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Anino.Models;

namespace Anino.Services;

public class EndpointDiscoveryService : IEndpointDiscoveryService
{
    private readonly IConsoleOutput _consoleOutput;
    private readonly IDtoAnalyzer _dtoAnalyzer;

    public EndpointDiscoveryService(IConsoleOutput consoleOutput, IDtoAnalyzer dtoAnalyzer)
    {
        _consoleOutput = consoleOutput;
        _dtoAnalyzer = dtoAnalyzer;
    }

    public IEnumerable<DiscoveredEndpoint> DiscoverEndpoints(IEnumerable<SyntaxTree> syntaxTrees)
    {
        var endpoints = new List<DiscoveredEndpoint>();

        foreach (var syntaxTree in syntaxTrees)
        {
            var fileName = Path.GetFileName(syntaxTree.FilePath ?? "unknown");
            _consoleOutput.WriteInformation($"Scanning {fileName}...");

            var root = syntaxTree.GetCompilationUnitRoot();
            
            // Discover Minimal API endpoints
            endpoints.AddRange(DiscoverMinimalApiEndpoints(root, syntaxTrees));
            
            // Discover MVC Controller endpoints
            endpoints.AddRange(DiscoverControllerEndpoints(root, syntaxTrees));
        }

        return endpoints;
    }

    public IEnumerable<DiscoveredEndpoint> DiscoverEndpoints(CSharpCompilation compilation)
    {
        var endpoints = new List<DiscoveredEndpoint>();

        foreach (var syntaxTree in compilation.SyntaxTrees)
        {
            var fileName = Path.GetFileName(syntaxTree.FilePath ?? "unknown");
            _consoleOutput.WriteInformation($"Scanning {fileName}...");

            var root = syntaxTree.GetCompilationUnitRoot();
            
            // Discover Minimal API endpoints with compilation context
            endpoints.AddRange(DiscoverMinimalApiEndpoints(root, compilation));
            
            // Discover MVC Controller endpoints with compilation context
            endpoints.AddRange(DiscoverControllerEndpoints(root, compilation));
        }

        return endpoints;
    }

    public IEnumerable<DiscoveredEndpoint> DiscoverEndpoints(CSharpCompilation compilation, IEnumerable<string>? targetControllers)
    {
        var endpoints = new List<DiscoveredEndpoint>();
        var targets = targetControllers?.ToHashSet(StringComparer.OrdinalIgnoreCase) ?? new HashSet<string>();
        var hasTargets = targets.Any();

        foreach (var syntaxTree in compilation.SyntaxTrees)
        {
            var fileName = Path.GetFileName(syntaxTree.FilePath ?? "unknown");
            
            // For minimal APIs (Program.cs), always include if no targets specified or if any targets specified
            if (fileName.Equals("Program.cs", StringComparison.OrdinalIgnoreCase))
            {
                if (!hasTargets)
                {
                    _consoleOutput.WriteInformation($"Scanning {fileName}...");
                    var root = syntaxTree.GetCompilationUnitRoot();
                    endpoints.AddRange(DiscoverMinimalApiEndpoints(root, compilation));
                }
                continue;
            }

            // For controller files, check if they match targets
            if (hasTargets)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                if (!targets.Contains(fileNameWithoutExtension))
                {
                    continue; // Skip this file as it's not in targets
                }
            }

            _consoleOutput.WriteInformation($"Scanning {fileName}...");
            var rootSyntax = syntaxTree.GetCompilationUnitRoot();
            
            // Discover Minimal API endpoints with compilation context
            endpoints.AddRange(DiscoverMinimalApiEndpoints(rootSyntax, compilation));
            
            // Discover MVC Controller endpoints with compilation context
            endpoints.AddRange(DiscoverControllerEndpoints(rootSyntax, compilation));
        }

        return endpoints;
    }

    private IEnumerable<DiscoveredEndpoint> DiscoverMinimalApiEndpoints(CompilationUnitSyntax root, IEnumerable<SyntaxTree> syntaxTrees)
    {
        var endpoints = new List<DiscoveredEndpoint>();

        // Look for app.MapGet, app.MapPost, etc.
        var invocationExpressions = root.DescendantNodes()
            .OfType<InvocationExpressionSyntax>()
            .Where(invocation => IsMinimalApiMapping(invocation));

        foreach (var invocation in invocationExpressions)
        {
            var endpoint = ParseMinimalApiEndpoint(invocation, syntaxTrees);
            if (endpoint != null)
            {
                endpoints.Add(endpoint);
            }
        }

        return endpoints;
    }

    private IEnumerable<DiscoveredEndpoint> DiscoverMinimalApiEndpoints(CompilationUnitSyntax root, CSharpCompilation compilation)
    {
        var endpoints = new List<DiscoveredEndpoint>();

        // Look for app.MapGet, app.MapPost, etc.
        var invocationExpressions = root.DescendantNodes()
            .OfType<InvocationExpressionSyntax>()
            .Where(invocation => IsMinimalApiMapping(invocation));

        foreach (var invocation in invocationExpressions)
        {
            var endpoint = ParseMinimalApiEndpoint(invocation, compilation);
            if (endpoint != null)
            {
                endpoints.Add(endpoint);
            }
        }

        return endpoints;
    }

    private IEnumerable<DiscoveredEndpoint> DiscoverControllerEndpoints(CompilationUnitSyntax root, IEnumerable<SyntaxTree> syntaxTrees)
    {
        var endpoints = new List<DiscoveredEndpoint>();

        // Find controller classes
        var controllerClasses = root.DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .Where(c => IsControllerClass(c));

        foreach (var controllerClass in controllerClasses)
        {
            var controllerRoute = GetControllerRoute(controllerClass);
            
            // Find action methods with HTTP attributes
            var actionMethods = controllerClass.Members
                .OfType<MethodDeclarationSyntax>()
                .Where(m => HasHttpAttribute(m));

            foreach (var method in actionMethods)
            {
                var endpoint = ParseControllerEndpoint(method, controllerRoute, syntaxTrees);
                if (endpoint != null)
                {
                    endpoints.Add(endpoint);
                }
            }
        }

        return endpoints;
    }

    private bool IsMinimalApiMapping(InvocationExpressionSyntax invocation)
    {
        if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
        {
            var methodName = memberAccess.Name.Identifier.ValueText;
            var httpMethods = new[] { "MapGet", "MapPost", "MapPut", "MapDelete", "MapPatch" };
            return httpMethods.Contains(methodName);
        }
        return false;
    }

    private DiscoveredEndpoint? ParseMinimalApiEndpoint(InvocationExpressionSyntax invocation, IEnumerable<SyntaxTree> syntaxTrees)
    {
        if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
            return null;

        var methodName = memberAccess.Name.Identifier.ValueText;
        var httpMethod = methodName.Replace("Map", "").ToUpperInvariant();

        // Get the route pattern (first argument)
        if (invocation.ArgumentList.Arguments.Count == 0)
            return null;

        var routeArgument = invocation.ArgumentList.Arguments[0];
        var routePattern = ExtractStringLiteral(routeArgument.Expression);

        if (string.IsNullOrEmpty(routePattern))
            return null;

        // Try to extract return type from the lambda expression (second argument)
        AnalyzedTypeInfo? returnTypeInfo = null;
        string returnType = "object";
        
        if (invocation.ArgumentList.Arguments.Count > 1)
        {
            var lambdaArgument = invocation.ArgumentList.Arguments[1];
            var lambdaExpression = ExtractLambdaExpression(lambdaArgument);
            if (lambdaExpression != null)
            {
                var returnTypeNode = ExtractReturnTypeFromLambda(lambdaExpression);
                if (returnTypeNode != null)
                {
                    returnType = returnTypeNode.ToString();
                    returnTypeInfo = _dtoAnalyzer.AnalyzeReturnType(returnTypeNode, syntaxTrees);
                }
            }
        }

        return new DiscoveredEndpoint
        {
            Path = routePattern,
            Method = httpMethod,
            ReturnType = returnType,
            StatusCode = GetDefaultStatusCode(httpMethod),
            ReturnTypeInfo = returnTypeInfo
        };
    }

    private IEnumerable<DiscoveredEndpoint> DiscoverControllerEndpoints(CompilationUnitSyntax root, CSharpCompilation compilation)
    {
        var endpoints = new List<DiscoveredEndpoint>();

        // Find controller classes
        var controllerClasses = root.DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .Where(c => IsControllerClass(c));

        foreach (var controllerClass in controllerClasses)
        {
            var controllerRoute = GetControllerRoute(controllerClass);
            
            // Find action methods with HTTP attributes
            var actionMethods = controllerClass.Members
                .OfType<MethodDeclarationSyntax>()
                .Where(m => HasHttpAttribute(m));

            foreach (var method in actionMethods)
            {
                var endpoint = ParseControllerEndpoint(method, controllerRoute, compilation);
                if (endpoint != null)
                {
                    endpoints.Add(endpoint);
                }
            }
        }

        return endpoints;
    }

    private DiscoveredEndpoint? ParseMinimalApiEndpoint(InvocationExpressionSyntax invocation, CSharpCompilation compilation)
    {
        if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
            return null;

        var methodName = memberAccess.Name.Identifier.ValueText;
        var httpMethod = methodName.Replace("Map", "").ToUpperInvariant();

        // Get the route pattern (first argument)
        if (invocation.ArgumentList.Arguments.Count == 0)
            return null;

        var routeArgument = invocation.ArgumentList.Arguments[0];
        var routePattern = ExtractStringLiteral(routeArgument.Expression);

        if (string.IsNullOrEmpty(routePattern))
            return null;

        // Try to extract return type from the lambda expression (second argument)
        AnalyzedTypeInfo? returnTypeInfo = null;
        string returnType = "object";
        
        if (invocation.ArgumentList.Arguments.Count > 1)
        {
            var lambdaArgument = invocation.ArgumentList.Arguments[1];
            var lambdaExpression = ExtractLambdaExpression(lambdaArgument);
            if (lambdaExpression != null)
            {
                var returnTypeNode = ExtractReturnTypeFromLambda(lambdaExpression);
                if (returnTypeNode != null)
                {
                    returnType = returnTypeNode.ToString();
                    returnTypeInfo = _dtoAnalyzer.AnalyzeReturnType(returnTypeNode, compilation);
                }
            }
        }

        return new DiscoveredEndpoint
        {
            Path = routePattern,
            Method = httpMethod,
            ReturnType = returnType,
            StatusCode = GetDefaultStatusCode(httpMethod),
            ReturnTypeInfo = returnTypeInfo
        };
    }

    private DiscoveredEndpoint? ParseControllerEndpoint(MethodDeclarationSyntax method, string controllerRoute, CSharpCompilation compilation)
    {
        // Find HTTP attribute
        var httpAttribute = method.AttributeLists
            .SelectMany(al => al.Attributes)
            .FirstOrDefault(attr => attr.Name.ToString().Contains("Http"));

        if (httpAttribute == null)
            return null;

        var httpMethod = ExtractHttpMethod(httpAttribute.Name.ToString());
        var methodRoute = ExtractRouteFromHttpAttribute(httpAttribute);
        
        var fullRoute = CombineRoutes(controllerRoute, methodRoute ?? "");

        // Extract return type information using compilation
        var returnTypeNode = method.ReturnType;
        var returnTypeInfo = _dtoAnalyzer.AnalyzeReturnType(returnTypeNode, compilation);

        return new DiscoveredEndpoint
        {
            Path = fullRoute,
            Method = httpMethod,
            ReturnType = method.ReturnType.ToString(),
            IsAsync = method.Modifiers.Any(m => m.IsKind(SyntaxKind.AsyncKeyword)),
            StatusCode = GetDefaultStatusCode(httpMethod),
            ReturnTypeInfo = returnTypeInfo
        };
    }

    private bool IsControllerClass(ClassDeclarationSyntax classDeclaration)
    {
        var className = classDeclaration.Identifier.ValueText;
        
        // Check if class name ends with "Controller"
        if (className.EndsWith("Controller"))
            return true;

        // Check if class has [ApiController] or [Controller] attribute
        return classDeclaration.AttributeLists
            .SelectMany(al => al.Attributes)
            .Any(attr => attr.Name.ToString().Contains("Controller") || 
                        attr.Name.ToString().Contains("ApiController"));
    }

    private string GetControllerRoute(ClassDeclarationSyntax controllerClass)
    {
        // Look for [Route] attribute
        var routeAttribute = controllerClass.AttributeLists
            .SelectMany(al => al.Attributes)
            .FirstOrDefault(attr => attr.Name.ToString().Contains("Route"));

        if (routeAttribute?.ArgumentList?.Arguments.Count > 0)
        {
            var routeTemplate = ExtractStringLiteral(routeAttribute.ArgumentList.Arguments[0].Expression) ?? "";
            return ResolveControllerRoute(routeTemplate, controllerClass.Identifier.ValueText);
        }

        // Default route based on controller name
        var controllerName = controllerClass.Identifier.ValueText;
        if (controllerName.EndsWith("Controller"))
        {
            controllerName = controllerName.Substring(0, controllerName.Length - 10);
        }

        return $"/api/{controllerName.ToLowerInvariant()}";
    }

    private string ResolveControllerRoute(string routeTemplate, string controllerClassName)
    {
        var controllerName = controllerClassName;
        if (controllerName.EndsWith("Controller"))
        {
            controllerName = controllerName.Substring(0, controllerName.Length - 10);
        }

        return routeTemplate.Replace("[controller]", controllerName.ToLowerInvariant());
    }

    private bool HasHttpAttribute(MethodDeclarationSyntax method)
    {
        var httpAttributes = new[] { "HttpGet", "HttpPost", "HttpPut", "HttpDelete", "HttpPatch" };
        
        return method.AttributeLists
            .SelectMany(al => al.Attributes)
            .Any(attr => httpAttributes.Any(httpAttr => attr.Name.ToString().Contains(httpAttr)));
    }

    private DiscoveredEndpoint? ParseControllerEndpoint(MethodDeclarationSyntax method, string controllerRoute, IEnumerable<SyntaxTree> syntaxTrees)
    {
        // Find HTTP attribute
        var httpAttribute = method.AttributeLists
            .SelectMany(al => al.Attributes)
            .FirstOrDefault(attr => attr.Name.ToString().Contains("Http"));

        if (httpAttribute == null)
            return null;

        var httpMethod = ExtractHttpMethod(httpAttribute.Name.ToString());
        var methodRoute = ExtractRouteFromHttpAttribute(httpAttribute);
        
        var fullRoute = CombineRoutes(controllerRoute, methodRoute ?? "");

        // Extract return type information
        var returnTypeNode = method.ReturnType;
        var returnTypeInfo = _dtoAnalyzer.AnalyzeReturnType(returnTypeNode, syntaxTrees);

        return new DiscoveredEndpoint
        {
            Path = fullRoute,
            Method = httpMethod,
            ReturnType = method.ReturnType.ToString(),
            IsAsync = method.Modifiers.Any(m => m.IsKind(SyntaxKind.AsyncKeyword)),
            StatusCode = GetDefaultStatusCode(httpMethod),
            ReturnTypeInfo = returnTypeInfo
        };
    }

    private string ExtractHttpMethod(string attributeName)
    {
        return attributeName switch
        {
            var name when name.Contains("HttpGet") => "GET",
            var name when name.Contains("HttpPost") => "POST",
            var name when name.Contains("HttpPut") => "PUT",
            var name when name.Contains("HttpDelete") => "DELETE",
            var name when name.Contains("HttpPatch") => "PATCH",
            _ => "GET"
        };
    }

    private string? ExtractRouteFromHttpAttribute(AttributeSyntax attribute)
    {
        if (attribute.ArgumentList?.Arguments.Count > 0)
        {
            return ExtractStringLiteral(attribute.ArgumentList.Arguments[0].Expression);
        }
        return null;
    }

    private string? ExtractStringLiteral(ExpressionSyntax expression)
    {
        if (expression is LiteralExpressionSyntax literal && 
            literal.Token.IsKind(SyntaxKind.StringLiteralToken))
        {
            return literal.Token.ValueText;
        }
        return null;
    }

    private string CombineRoutes(string baseRoute, string actionRoute)
    {
        if (string.IsNullOrEmpty(actionRoute))
            return baseRoute;

        baseRoute = baseRoute.TrimEnd('/');
        actionRoute = actionRoute.TrimStart('/');

        return $"{baseRoute}/{actionRoute}";
    }

    private int GetDefaultStatusCode(string httpMethod)
    {
        return httpMethod.ToUpperInvariant() switch
        {
            "POST" => 201,
            "DELETE" => 204,
            _ => 200
        };
    }

    private SyntaxNode? ExtractLambdaExpression(ArgumentSyntax argument)
    {
        return argument.Expression switch
        {
            SimpleLambdaExpressionSyntax simpleLambda => simpleLambda,
            ParenthesizedLambdaExpressionSyntax parenLambda => parenLambda,
            _ => null
        };
    }

    private SyntaxNode? ExtractReturnTypeFromLambda(SyntaxNode lambdaExpression)
    {
        if (lambdaExpression is SimpleLambdaExpressionSyntax simpleLambda)
        {
            return ExtractReturnTypeFromExpression(simpleLambda.Body);
        }
        else if (lambdaExpression is ParenthesizedLambdaExpressionSyntax parenLambda)
        {
            return ExtractReturnTypeFromExpression(parenLambda.Body);
        }

        return null;
    }

    private SyntaxNode? ExtractReturnTypeFromExpression(SyntaxNode expression)
    {
        return expression switch
        {
            // Handle: () => new List<Book>()
            ObjectCreationExpressionSyntax objCreation => objCreation.Type,
            
            // Handle: () => new List<Book> { ... }
            ImplicitObjectCreationExpressionSyntax implicitCreation => 
                // Try to infer from initializer
                null, // We'd need more complex analysis here
            
            // Handle: () => someVariable (where someVariable is typed)
            IdentifierNameSyntax identifier => 
                // We'd need semantic analysis to get the type
                null,
            
            // Handle: () => SomeMethod() - would need semantic analysis
            InvocationExpressionSyntax invocation => 
                null,
            
            // Handle block expressions like () => { return new List<Book>(); }
            BlockSyntax block => ExtractReturnTypeFromBlock(block),
            
            _ => null
        };
    }

    private SyntaxNode? ExtractReturnTypeFromBlock(BlockSyntax block)
    {
        // Look for return statements
        var returnStatements = block.DescendantNodes()
            .OfType<ReturnStatementSyntax>()
            .FirstOrDefault();

        if (returnStatements?.Expression != null)
        {
            return ExtractReturnTypeFromExpression(returnStatements.Expression);
        }

        return null;
    }
}