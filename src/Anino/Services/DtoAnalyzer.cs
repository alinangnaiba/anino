using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Anino.Models;

namespace Anino.Services;

public class DtoAnalyzer : IDtoAnalyzer
{
    public AnalyzedTypeInfo? AnalyzeReturnType(SyntaxNode returnTypeNode, IEnumerable<SyntaxTree> syntaxTrees)
    {
        if (returnTypeNode == null) 
        {
            return null;
        }

        var typeName = ExtractTypeName(returnTypeNode);
        if (string.IsNullOrEmpty(typeName))
        {
            return null;
        }

        // Handle generic return types like ActionResult<T>, Task<T>, etc.
        typeName = UnwrapGenericWrapperTypes(typeName);

        return AnalyzeType(typeName, syntaxTrees);
    }

    public AnalyzedTypeInfo? AnalyzeType(string typeName, IEnumerable<SyntaxTree> syntaxTrees)
    {
        return AnalyzeType(typeName, syntaxTrees, new HashSet<string>());
    }

    private AnalyzedTypeInfo? AnalyzeType(string typeName, IEnumerable<SyntaxTree> syntaxTrees, HashSet<string> visited)
    {
        if (string.IsNullOrEmpty(typeName))
        {
            return null;
        }

        var typeInfo = new AnalyzedTypeInfo { TypeName = typeName };

        // Handle common collection types
        if (IsCollectionType(typeName, out var elementType))
        {
            typeInfo.IsCollection = true;
            typeInfo.GenericArgument = elementType;
            
            // If the element type is a complex type, analyze its properties
            if (!IsPrimitiveType(elementType))
            {
                var elementTypeInfo = AnalyzeType(elementType, syntaxTrees, visited);
                if (elementTypeInfo != null)
                {
                    typeInfo.Properties = elementTypeInfo.Properties;
                }
            }
            
            return typeInfo; // Return early for collections
        }

        // Handle generic types
        if (IsGenericType(typeName, out var genericArg))
        {
            typeInfo.IsGeneric = true;
            typeInfo.GenericArgument = genericArg;
            typeName = genericArg; // Analyze the generic argument
        }

        // Skip primitive types - no need to analyze their properties
        if (IsPrimitiveType(typeName))
        {
            return typeInfo;
        }

        // Find the class/record definition in the syntax trees
        var classDeclaration = FindTypeDefinition(typeName, syntaxTrees);
        if (classDeclaration != null)
        {
            typeInfo.Properties = ExtractProperties(classDeclaration, syntaxTrees, visited);
        }

        return typeInfo;
    }

    private string ExtractTypeName(SyntaxNode node)
    {
        return node switch
        {
            IdentifierNameSyntax identifierName => identifierName.Identifier.ValueText,
            GenericNameSyntax genericName => genericName.ToString(), // Include full generic type with arguments
            QualifiedNameSyntax qualifiedName => qualifiedName.Right.ToString(),
            PredefinedTypeSyntax predefinedType => predefinedType.Keyword.ValueText,
            ArrayTypeSyntax arrayType => $"{ExtractTypeName(arrayType.ElementType)}[]",
            _ => node.ToString()
        };
    }

    private bool IsCollectionType(string typeName, out string elementType)
    {
        elementType = string.Empty;

        // Handle array types
        if (typeName.EndsWith("[]"))
        {
            elementType = typeName.Substring(0, typeName.Length - 2);
            return true;
        }

        // Skip Dictionary types - they should be handled as objects with dynamic key-value pairs, not collections
        var dictionaryTypes = new[] { "Dictionary", "IDictionary", "IReadOnlyDictionary" };
        foreach (var dictType in dictionaryTypes)
        {
            if (typeName.StartsWith($"{dictType}<") && typeName.EndsWith(">"))
            {
                return false; // Don't treat dictionaries as collections
            }
        }

        // Handle generic collections
        var collectionTypes = new[] 
        { 
            "List", "IList", "IEnumerable", "ICollection", "Array",
            "IReadOnlyList", "IReadOnlyCollection", "Collection",
            "ObservableCollection", "HashSet", "LinkedList", "Queue", "Stack"
        };
        
        foreach (var collectionType in collectionTypes)
        {
            if (typeName.StartsWith($"{collectionType}<") && typeName.EndsWith(">"))
            {
                var start = collectionType.Length + 1;
                var length = typeName.Length - start - 1;
                elementType = typeName.Substring(start, length);
                return true;
            }
        }

        // Handle non-generic collections
        var nonGenericCollections = new[] { "IEnumerable", "ICollection", "IList", "ArrayList" };
        if (nonGenericCollections.Contains(typeName))
        {
            elementType = "object"; // Non-generic collections contain objects
            return true;
        }

        return false;
    }

    private bool IsGenericType(string typeName, out string genericArgument)
    {
        genericArgument = string.Empty;

        if (typeName.Contains('<') && typeName.EndsWith('>'))
        {
            var start = typeName.IndexOf('<') + 1;
            var length = typeName.Length - start - 1;
            genericArgument = typeName.Substring(start, length);
            return true;
        }

        return false;
    }

    private bool IsPrimitiveType(string typeName)
    {
        var primitiveTypes = new[]
        {
            "string", "int", "long", "short", "byte", "sbyte",
            "uint", "ulong", "ushort", "float", "double", "decimal",
            "bool", "char", "DateTime", "DateOnly", "TimeOnly", "TimeSpan",
            "Guid", "object"
        };

        return primitiveTypes.Contains(typeName.TrimEnd('?')); // Handle nullable types
    }

    private SyntaxNode? FindTypeDefinition(string typeName, IEnumerable<SyntaxTree> syntaxTrees)
    {
        foreach (var syntaxTree in syntaxTrees)
        {
            var root = syntaxTree.GetCompilationUnitRoot();

            // Look for class declarations
            var classDeclaration = root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(c => c.Identifier.ValueText == typeName);

            if (classDeclaration != null)
            {
                return classDeclaration;
            }

            // Look for record declarations
            var recordDeclaration = root.DescendantNodes()
                .OfType<RecordDeclarationSyntax>()
                .FirstOrDefault(r => r.Identifier.ValueText == typeName);

            if (recordDeclaration != null)
            {
                return recordDeclaration;
            }
        }

        return null;
    }

    private List<AnalyzedPropertyInfo> ExtractProperties(SyntaxNode typeDeclaration, IEnumerable<SyntaxTree> syntaxTrees, HashSet<string> visited)
    {
        var properties = new List<AnalyzedPropertyInfo>();

        if (typeDeclaration is ClassDeclarationSyntax classDecl)
        {
            // Extract properties from class
            var propertyDeclarations = classDecl.Members.OfType<PropertyDeclarationSyntax>();
            
            foreach (var prop in propertyDeclarations)
            {
                var propertyInfo = CreatePropertyInfo(prop.Identifier.ValueText, prop.Type.ToString(), syntaxTrees, visited);
                properties.Add(propertyInfo);
            }
        }
        else if (typeDeclaration is RecordDeclarationSyntax recordDecl)
        {
            // Extract parameters from record
            if (recordDecl.ParameterList != null)
            {
                foreach (var param in recordDecl.ParameterList.Parameters)
                {
                    if (param.Type != null)
                    {
                        var propertyInfo = CreatePropertyInfo(param.Identifier.ValueText, param.Type.ToString(), syntaxTrees, visited);
                        properties.Add(propertyInfo);
                    }
                }
            }

            // Also extract properties from record body
            var propertyDeclarations = recordDecl.Members.OfType<PropertyDeclarationSyntax>();
            
            foreach (var prop in propertyDeclarations)
            {
                var propertyInfo = CreatePropertyInfo(prop.Identifier.ValueText, prop.Type.ToString(), syntaxTrees, visited);
                properties.Add(propertyInfo);
            }
        }

        return properties;
    }

    private AnalyzedPropertyInfo CreatePropertyInfo(string name, string type, IEnumerable<SyntaxTree> syntaxTrees, HashSet<string>? visited = null)
    {
        visited ??= new HashSet<string>();
        
        var propertyInfo = new AnalyzedPropertyInfo
        {
            Name = name,
            Type = type.TrimEnd('?'), // Remove nullable indicator
            IsNullable = type.EndsWith('?')
        };

        var cleanType = propertyInfo.Type;

        // Check if it's a collection
        if (IsCollectionType(type, out var elementType))
        {
            propertyInfo.IsCollection = true;
            propertyInfo.GenericArgument = elementType;
            
            // For collections, analyze the element type if it's complex
            if (!IsPrimitiveType(elementType) && !visited.Contains(elementType))
            {
                visited.Add(elementType);
                var elementTypeInfo = AnalyzeType(elementType, syntaxTrees, visited);
                if (elementTypeInfo?.Properties.Any() == true)
                {
                    propertyInfo.NestedProperties = elementTypeInfo.Properties;
                }
                visited.Remove(elementType);
            }
        }
        // Check if it's a complex type that needs nested analysis
        else if (!IsPrimitiveType(cleanType) && !visited.Contains(cleanType))
        {
            visited.Add(cleanType);
            var nestedTypeInfo = AnalyzeType(cleanType, syntaxTrees, visited);
            if (nestedTypeInfo?.Properties.Any() == true)
            {
                propertyInfo.NestedProperties = nestedTypeInfo.Properties;
            }
            visited.Remove(cleanType);
        }

        return propertyInfo;
    }

    private string UnwrapGenericWrapperTypes(string typeName)
    {
        // Handle common wrapper types that we need to unwrap
        var wrapperTypes = new[]
        {
            "ActionResult", "IActionResult", "Task", "ValueTask",
            "Response", "Result", "ApiResponse"
        };

        foreach (var wrapperType in wrapperTypes)
        {
            if (typeName.StartsWith($"{wrapperType}<") && typeName.EndsWith(">"))
            {
                var start = wrapperType.Length + 1;
                var length = typeName.Length - start - 1;
                var innerType = typeName.Substring(start, length);
                
                // Recursively unwrap in case there are nested wrappers like Task<ActionResult<List<User>>>
                return UnwrapGenericWrapperTypes(innerType);
            }
        }

        return typeName;
    }

    // New methods that use compilation/semantic model for better type resolution
    public AnalyzedTypeInfo? AnalyzeReturnType(SyntaxNode returnTypeNode, CSharpCompilation compilation)
    {
        if (returnTypeNode == null)
        {
            return null;
        }

        var typeName = ExtractTypeName(returnTypeNode);
        if (string.IsNullOrEmpty(typeName))
        {
            return null;
        }

        // Handle generic return types like ActionResult<T>, Task<T>, etc.
        typeName = UnwrapGenericWrapperTypes(typeName);

        return AnalyzeType(typeName, compilation);
    }

    public AnalyzedTypeInfo? AnalyzeType(string typeName, CSharpCompilation compilation)
    {
        return AnalyzeTypeWithSemanticModel(typeName, compilation, new HashSet<string>());
    }

    private AnalyzedTypeInfo? AnalyzeTypeWithSemanticModel(string typeName, CSharpCompilation compilation, HashSet<string> visited)
    {
        if (string.IsNullOrEmpty(typeName) || visited.Contains(typeName))
        {
            return null;
        }

        visited.Add(typeName);
        var typeInfo = new AnalyzedTypeInfo { TypeName = typeName };

        try
        {
            // Handle common collection types
            if (IsCollectionType(typeName, out var elementType))
            {
                typeInfo.IsCollection = true;
                typeInfo.GenericArgument = elementType;
                
                // If the element type is a complex type, analyze its properties
                if (!IsPrimitiveType(elementType))
                {
                    var elementTypeInfo = AnalyzeTypeWithSemanticModel(elementType, compilation, visited);
                    if (elementTypeInfo != null)
                    {
                        typeInfo.Properties = elementTypeInfo.Properties;
                    }
                }
                
                return typeInfo;
            }

            // Handle generic types
            if (IsGenericType(typeName, out var genericArg))
            {
                typeInfo.IsGeneric = true;
                typeInfo.GenericArgument = genericArg;
                typeName = genericArg; // Analyze the generic argument
            }

            // Skip primitive types
            if (IsPrimitiveType(typeName))
            {
                return typeInfo;
            }

            // Use semantic model to find the type symbol
            var typeSymbol = FindTypeSymbolInCompilation(typeName, compilation);
            if (typeSymbol != null)
            {
                // If we found the type symbol, analyze it properly including collection detection
                return AnalyzeTypeSymbol(typeSymbol, compilation, visited);
            }
            else
            {
                // Fallback to syntax tree search
                var classDeclaration = FindTypeDefinition(typeName, compilation.SyntaxTrees);
                if (classDeclaration != null)
                {
                    typeInfo.Properties = ExtractProperties(classDeclaration, compilation.SyntaxTrees, visited);
                }
            }

            return typeInfo;
        }
        finally
        {
            visited.Remove(typeName);
        }
    }

    private ITypeSymbol? FindTypeSymbolInCompilation(string typeName, CSharpCompilation compilation)
    {
        // Clean up the type name for various scenarios
        var cleanTypeName = CleanTypeName(typeName);
        
        // Strategy 1: Try direct metadata name lookup (works for fully qualified external types)
        ITypeSymbol? typeSymbol = compilation.GetTypeByMetadataName(cleanTypeName);
        if (typeSymbol != null)
        {
            return typeSymbol;
        }
        
        // Strategy 1.5: Try with global namespace prefix for types that might be in the global namespace
        typeSymbol = compilation.GetTypeByMetadataName($"global::{cleanTypeName}");
        if (typeSymbol != null)
        {
            return typeSymbol;
        }

        // Strategy 2: Try common namespace prefixes for external types
        var commonNamespaces = new[]
        {
            "System.Collections.Generic",
            "System",
            "Microsoft.AspNetCore.Mvc",
            "System.Threading.Tasks",
            "System.Collections",
            "System.Text.Json"
        };

        foreach (var ns in commonNamespaces)
        {
            var qualifiedName = $"{ns}.{cleanTypeName}";
            typeSymbol = compilation.GetTypeByMetadataName(qualifiedName);
            if (typeSymbol != null)
            {
                return typeSymbol;
            }
        }

        // Strategy 3: Search through syntax trees for internal types
        foreach (var syntaxTree in compilation.SyntaxTrees)
        {
            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            var root = syntaxTree.GetCompilationUnitRoot();

            // Search for type declarations
            var typeDeclarations = root.DescendantNodes()
                .Where(n => n is ClassDeclarationSyntax || n is RecordDeclarationSyntax || n is StructDeclarationSyntax)
                .Cast<BaseTypeDeclarationSyntax>();

            foreach (var typeDecl in typeDeclarations)
            {
                if (typeDecl.Identifier.ValueText == cleanTypeName)
                {
                    var symbol = semanticModel.GetDeclaredSymbol(typeDecl);
                    if (symbol is ITypeSymbol typeSymbolFound)
                    {
                        return typeSymbolFound;
                    }
                }
            }

            // Strategy 4: Use semantic model to resolve type references in using statements
            var usingDirectives = root.Usings;
            foreach (var usingDirective in usingDirectives)
            {
                if (usingDirective.Name != null)
                {
                    var namespaceName = usingDirective.Name.ToString();
                    var qualifiedName = $"{namespaceName}.{cleanTypeName}";
                    typeSymbol = compilation.GetTypeByMetadataName(qualifiedName);
                    if (typeSymbol != null)
                    {
                        return typeSymbol;
                    }
                }
            }
        }

        // Strategy 5: Search through all referenced assemblies
        foreach (var reference in compilation.References)
        {
            if (compilation.GetAssemblyOrModuleSymbol(reference) is IAssemblySymbol assembly)
            {
                typeSymbol = FindTypeInAssembly(assembly, cleanTypeName);
                if (typeSymbol != null)
                {
                    return typeSymbol;
                }
            }
        }

        // Strategy 6: Search through all namespaces in the main assembly (for local project types)
        typeSymbol = FindTypeInAssembly(compilation.Assembly, cleanTypeName);
        if (typeSymbol != null)
        {
            return typeSymbol;
        }

        return null;
    }

    private string CleanTypeName(string typeName)
    {
        // Remove nullable indicator
        typeName = typeName.TrimEnd('?');
        
        // Handle array types
        if (typeName.EndsWith("[]"))
        {
            typeName = typeName.Substring(0, typeName.Length - 2);
        }
        
        // Remove generic type parameters for lookup (we'll handle generics separately)
        var genericIndex = typeName.IndexOf('<');
        if (genericIndex > 0)
        {
            typeName = typeName.Substring(0, genericIndex);
        }
        
        return typeName;
    }

    private ITypeSymbol? FindTypeInAssembly(IAssemblySymbol assembly, string typeName)
    {
        return FindTypeInNamespace(assembly.GlobalNamespace, typeName);
    }

    private ITypeSymbol? FindTypeInNamespace(INamespaceSymbol namespaceSymbol, string typeName)
    {
        // Search for types in this namespace
        foreach (var member in namespaceSymbol.GetMembers())
        {
            if (member is ITypeSymbol typeSymbol && typeSymbol.Name == typeName)
            {
                return typeSymbol;
            }
            
            // Recursively search nested namespaces
            if (member is INamespaceSymbol nestedNamespace)
            {
                var result = FindTypeInNamespace(nestedNamespace, typeName);
                if (result != null)
                {
                    return result;
                }
            }
        }
        
        return null;
    }

    private List<AnalyzedPropertyInfo> ExtractPropertiesFromSymbol(ITypeSymbol typeSymbol, CSharpCompilation compilation, HashSet<string> visited)
    {
        var properties = new List<AnalyzedPropertyInfo>();

        // Get all accessible properties (including inherited ones)
        var allMembers = GetAllAccessibleMembers(typeSymbol);
        
        foreach (var member in allMembers)
        {
            if (member is IPropertySymbol property && IsPublicProperty(property))
            {
                var propertyInfo = CreatePropertyInfoFromSymbol(property, compilation, visited);
                properties.Add(propertyInfo);
            }
        }

        return properties;
    }

    private IEnumerable<ISymbol> GetAllAccessibleMembers(ITypeSymbol typeSymbol)
    {
        var members = new List<ISymbol>();
        
        // Add current type members
        members.AddRange(typeSymbol.GetMembers());
        
        // Add inherited members from base types (for external types like DTOs that inherit from base classes)
        var currentType = typeSymbol.BaseType;
        while (currentType != null && currentType.SpecialType != SpecialType.System_Object)
        {
            members.AddRange(currentType.GetMembers().Where(m => m.DeclaredAccessibility == Accessibility.Public));
            currentType = currentType.BaseType;
        }
        
        return members;
    }

    private bool IsPublicProperty(IPropertySymbol property)
    {
        // Only include public properties that have both getter and setter (or at least getter)
        return property.DeclaredAccessibility == Accessibility.Public && 
               property.GetMethod != null && 
               property.GetMethod.DeclaredAccessibility == Accessibility.Public;
    }

    private AnalyzedPropertyInfo CreatePropertyInfoFromSymbol(IPropertySymbol property, CSharpCompilation compilation, HashSet<string> visited)
    {
        var propertyInfo = new AnalyzedPropertyInfo
        {
            Name = property.Name,
            Type = property.Type.ToDisplayString(),
            IsNullable = property.Type.CanBeReferencedByName && property.NullableAnnotation == NullableAnnotation.Annotated
        };

        var typeName = property.Type.Name;
        var fullTypeName = property.Type.ToDisplayString();

        // Handle collection types (List<T>, IEnumerable<T>, etc.)
        var isCollection = IsCollectionTypeSymbol(property.Type, out var elementTypeSymbol);
        
        
        if (isCollection)
        {
            propertyInfo.IsCollection = true;
            propertyInfo.GenericArgument = elementTypeSymbol.Name;
            
            // For collections, analyze the element type if it's complex
            var elementFullTypeName = elementTypeSymbol.ToDisplayString();
            if (!IsPrimitiveTypeSymbol(elementTypeSymbol) && !visited.Contains(elementFullTypeName))
            {
                // Directly extract properties from the element type symbol
                visited.Add(elementFullTypeName);
                try
                {
                    var elementProperties = ExtractPropertiesFromSymbol(elementTypeSymbol, compilation, visited);
                    if (elementProperties.Any())
                    {
                        propertyInfo.NestedProperties = elementProperties;
                    }
                }
                finally
                {
                    visited.Remove(elementFullTypeName);
                }
            }
        }
        // Check if it's a Dictionary type that needs special handling
        else if (IsDictionaryType(property.Type, out var keyType, out var valueType))
        {
            propertyInfo.IsDictionary = true;
            propertyInfo.GenericArgument = valueType.Name; // Store the value type for mock generation
            
            // For dictionaries, we don't analyze nested properties since they're dynamic
            // The mock generator will handle creating key-value pairs
        }
        // Check if it's a complex type that needs nested analysis
        else if (!IsPrimitiveTypeSymbol(property.Type) && !visited.Contains(typeName))
        {
            var nestedTypeInfo = AnalyzeTypeSymbol(property.Type, compilation, visited);
            if (nestedTypeInfo?.Properties.Any() == true)
            {
                propertyInfo.NestedProperties = nestedTypeInfo.Properties;
            }
        }

        return propertyInfo;
    }

    private bool IsCollectionTypeSymbol(ITypeSymbol typeSymbol, out ITypeSymbol elementType)
    {
        elementType = null!;

        // Handle arrays
        if (typeSymbol is IArrayTypeSymbol arrayType)
        {
            elementType = arrayType.ElementType;
            return true;
        }

        // Handle Dictionary<TKey, TValue> - these should NOT be treated as collections
        // They should be handled as objects with dynamic properties
        if (typeSymbol is INamedTypeSymbol namedType && namedType.IsGenericType)
        {
            var constructedFrom = namedType.ConstructedFrom.ToDisplayString();
            
            // Skip Dictionary types - they should be handled as objects, not collections
            if (constructedFrom == "System.Collections.Generic.Dictionary<,>" ||
                constructedFrom == "System.Collections.Generic.IDictionary<,>" ||
                constructedFrom == "System.Collections.Generic.IReadOnlyDictionary<,>" ||
                constructedFrom == "Dictionary<, >" ||
                constructedFrom == "IDictionary<, >" ||
                constructedFrom == "IReadOnlyDictionary<, >")
            {
                return false; // Don't treat dictionaries as collections
            }

            // First, check for well-known collection types directly
            var collectionTypeNames = new[] { "List<>", "IList<>", "ICollection<>", "IReadOnlyList<>", "IReadOnlyCollection<>" };
            if (collectionTypeNames.Contains(constructedFrom))
            {
                elementType = namedType.TypeArguments[0];
                return true;
            }
            
            // Then check if the type implements IEnumerable<T>
            var enumerableInterface = FindGenericEnumerableInterface(namedType);
            if (enumerableInterface != null)
            {
                elementType = enumerableInterface.TypeArguments[0];
                return true;
            }
        }

        // Handle non-generic collections (IEnumerable, ICollection, IList)
        if (ImplementsNonGenericCollection(typeSymbol))
        {
            // For non-generic collections, we don't know the element type, so use object
            elementType = typeSymbol; // This will be handled as object in mock generation
            return true;
        }

        return false;
    }

    private bool IsDictionaryType(ITypeSymbol typeSymbol, out ITypeSymbol keyType, out ITypeSymbol valueType)
    {
        keyType = null!;
        valueType = null!;

        if (typeSymbol is INamedTypeSymbol namedType && namedType.IsGenericType)
        {
            var constructedFrom = namedType.ConstructedFrom.ToDisplayString();
            
            // Check for Dictionary types
            if (constructedFrom == "System.Collections.Generic.Dictionary<,>" ||
                constructedFrom == "System.Collections.Generic.IDictionary<,>" ||
                constructedFrom == "System.Collections.Generic.IReadOnlyDictionary<,>" ||
                constructedFrom == "Dictionary<, >" ||
                constructedFrom == "IDictionary<, >" ||
                constructedFrom == "IReadOnlyDictionary<, >")
            {
                if (namedType.TypeArguments.Length >= 2)
                {
                    keyType = namedType.TypeArguments[0];
                    valueType = namedType.TypeArguments[1];
                    return true;
                }
            }
        }

        return false;
    }

    private INamedTypeSymbol? FindGenericEnumerableInterface(ITypeSymbol typeSymbol)
    {
        // Check if the type itself is IEnumerable<T>
        if (typeSymbol is INamedTypeSymbol namedType && 
            namedType.IsGenericType)
        {
            var constructedFrom = namedType.ConstructedFrom.ToDisplayString();
            if (constructedFrom == "System.Collections.Generic.IEnumerable<>" || 
                constructedFrom == "IEnumerable<>")
            {
                return namedType;
            }
        }

        // Check all implemented interfaces for IEnumerable<T>
        foreach (var interfaceType in typeSymbol.AllInterfaces)
        {
            if (interfaceType.IsGenericType)
            {
                var constructedFrom = interfaceType.ConstructedFrom.ToDisplayString();
                if (constructedFrom == "System.Collections.Generic.IEnumerable<>" || 
                    constructedFrom == "IEnumerable<>")
                {
                    return interfaceType;
                }
            }
        }

        return null;
    }

    private bool ImplementsNonGenericCollection(ITypeSymbol typeSymbol)
    {
        var nonGenericCollectionTypes = new[]
        {
            "System.Collections.IEnumerable",
            "System.Collections.ICollection",
            "System.Collections.IList"
        };

        // Check if the type itself is a non-generic collection
        var fullTypeName = typeSymbol.ToDisplayString();
        if (nonGenericCollectionTypes.Contains(fullTypeName))
        {
            return true;
        }

        // Check all implemented interfaces
        foreach (var interfaceType in typeSymbol.AllInterfaces)
        {
            var interfaceName = interfaceType.ToDisplayString();
            if (nonGenericCollectionTypes.Contains(interfaceName))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsPrimitiveTypeSymbol(ITypeSymbol typeSymbol)
    {
        // Handle special types
        switch (typeSymbol.SpecialType)
        {
            case SpecialType.System_Boolean:
            case SpecialType.System_Byte:
            case SpecialType.System_SByte:
            case SpecialType.System_Int16:
            case SpecialType.System_UInt16:
            case SpecialType.System_Int32:
            case SpecialType.System_UInt32:
            case SpecialType.System_Int64:
            case SpecialType.System_UInt64:
            case SpecialType.System_Decimal:
            case SpecialType.System_Single:
            case SpecialType.System_Double:
            case SpecialType.System_Char:
            case SpecialType.System_String:
            case SpecialType.System_Object:
                return true;
        }

        // Handle common types by name
        var typeName = typeSymbol.ToDisplayString();
        var primitiveTypes = new[]
        {
            "System.DateTime", "System.DateOnly", "System.TimeOnly", "System.TimeSpan",
            "System.Guid", "System.Uri"
        };

        return primitiveTypes.Contains(typeName);
    }

    private AnalyzedTypeInfo? AnalyzeTypeSymbol(ITypeSymbol typeSymbol, CSharpCompilation compilation, HashSet<string> visited)
    {
        var typeName = typeSymbol.Name;
        var fullTypeName = typeSymbol.ToDisplayString();
        
        if (visited.Contains(fullTypeName))
        {
            return null;
        }

        visited.Add(fullTypeName);
        
        try
        {
            var typeInfo = new AnalyzedTypeInfo { TypeName = typeName };

            // Check if this type itself is a collection (but not Dictionary)
            if (IsCollectionTypeSymbol(typeSymbol, out var elementTypeSymbol) && !IsDictionaryType(typeSymbol, out _, out _))
            {
                typeInfo.IsCollection = true;
                typeInfo.GenericArgument = elementTypeSymbol.Name;
                
                // If the element type is complex, analyze its properties
                if (!IsPrimitiveTypeSymbol(elementTypeSymbol))
                {
                    var elementTypeInfo = AnalyzeTypeSymbol(elementTypeSymbol, compilation, visited);
                    if (elementTypeInfo?.Properties.Any() == true)
                    {
                        typeInfo.Properties = elementTypeInfo.Properties;
                    }
                }
            }
            // Check if this type itself is a Dictionary
            else if (IsDictionaryType(typeSymbol, out var keyTypeSymbol, out var valueTypeSymbol))
            {
                // Mark as Dictionary type and store the value type for generation
                typeInfo.TypeName = $"Dictionary<{keyTypeSymbol.Name}, {valueTypeSymbol.Name}>";
                typeInfo.GenericArgument = valueTypeSymbol.Name; // Store value type name
                
                // If value type is complex (not primitive), try to analyze its properties
                if (!IsPrimitiveTypeSymbol(valueTypeSymbol))
                {
                    var valueTypeName = valueTypeSymbol.ToDisplayString();
                    var valueTypeInfo = AnalyzeTypeWithSemanticModel(valueTypeName, compilation, new HashSet<string>(visited));
                    if (valueTypeInfo?.Properties.Any() == true)
                    {
                        // Store value type properties for Dictionary generation
                        typeInfo.Properties = valueTypeInfo.Properties;
                    }
                }
            }
            else
            {
                // Not a collection or dictionary, analyze as regular type
                typeInfo.Properties = ExtractPropertiesFromSymbol(typeSymbol, compilation, visited);
            }
            
            return typeInfo;
        }
        finally
        {
            visited.Remove(fullTypeName);
        }
    }
}