using Anino.Models;

namespace Anino.Services;

public class MockDataGenerator : IMockDataGenerator
{
    private readonly Random _random = new();

    public object GenerateMockResponse(DiscoveredEndpoint endpoint)
    {
        // For DELETE methods, return null for 204 status codes
        if (endpoint.Method.ToUpperInvariant() == "DELETE" && endpoint.StatusCode == 204)
        {
            return null!;
        }

        // If we have analyzed type information, use it
        if (endpoint.ReturnTypeInfo != null)
        {
            return GenerateFromTypeInfo(endpoint.ReturnTypeInfo);
        }

        // Fallback to simple method-based generation
        return GenerateSimpleFallback(endpoint.Method);
    }

    private object GenerateFromTypeInfo(AnalyzedTypeInfo typeInfo)
    {
        // Handle collections
        if (typeInfo.IsCollection)
        {
            return GenerateCollection(typeInfo);
        }

        // Handle dictionaries at the type level
        if (IsDictionaryType(typeInfo.TypeName))
        {
            return GenerateTypeLevelDictionary(typeInfo);
        }

        // Handle primitive types
        if (IsPrimitiveType(typeInfo.TypeName))
        {
            return GeneratePrimitiveValue(typeInfo.TypeName);
        }

        // Handle complex objects with properties
        if (typeInfo.Properties.Any())
        {
            return GenerateObjectFromProperties(typeInfo.Properties);
        }

        // Fallback for unknown types
        return GeneratePrimitiveValue("unknown");
    }

    private object GenerateCollection(AnalyzedTypeInfo typeInfo)
    {
        var itemCount = _random.Next(2, 4); // Generate 2-3 items
        var items = new List<object>();

        for (int i = 0; i < itemCount; i++)
        {
            if (!string.IsNullOrEmpty(typeInfo.GenericArgument))
            {
                // Create a mock AnalyzedTypeInfo for the collection element
                var elementTypeInfo = new AnalyzedTypeInfo
                {
                    TypeName = typeInfo.GenericArgument,
                    Properties = typeInfo.Properties // Inherit properties if it's a complex type
                };

                items.Add(GenerateFromTypeInfo(elementTypeInfo));
            }
            else
            {
                // Generate generic items
                items.Add(new
                {
                    id = i + 1,
                    name = $"Item {i + 1}",
                    value = _random.Next(1, 1000)
                });
            }
        }

        return items;
    }

    private object GenerateDictionaryValue(AnalyzedPropertyInfo property)
    {
        var dict = new Dictionary<string, object>();
        var keyCount = _random.Next(2, 4); // Generate 2-3 key-value pairs

        for (int i = 0; i < keyCount; i++)
        {
            var key = GenerateDictionaryKey(i);
            object value;
            
            // If we have nested properties, it means the value type is complex
            if (property.NestedProperties?.Any() == true)
            {
                value = GenerateObjectFromProperties(property.NestedProperties);
            }
            else
            {
                var valueType = !string.IsNullOrEmpty(property.GenericArgument) 
                    ? property.GenericArgument 
                    : "string";
                    
                // If value type is not a primitive, create a fallback object structure
                if (!IsPrimitiveType(valueType))
                {
                    value = new
                    {
                        id = _random.Next(1, 1000),
                        name = GenerateStringValue(),
                        value = _random.NextDouble() * 1000,
                        isActive = _random.Next(0, 2) == 1,
                        metadata = new Dictionary<string, object>
                        {
                            ["type"] = valueType,
                            ["generated"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                        }
                    };
                }
                else
                {
                    value = GeneratePrimitiveValue(valueType);
                }
            }
                
            dict[key] = value;
        }

        return dict;
    }

    private string GenerateDictionaryKey(int index)
    {
        var keyPrefixes = new[] { "metric", "value", "key", "item", "data" };
        var prefix = keyPrefixes[index % keyPrefixes.Length];
        return $"{prefix}{index + 1}";
    }

    private bool IsDictionaryType(string typeName)
    {
        return typeName.StartsWith("Dictionary<") || 
               typeName.StartsWith("IDictionary<") || 
               typeName.StartsWith("IReadOnlyDictionary<");
    }

    private object GenerateTypeLevelDictionary(AnalyzedTypeInfo typeInfo)
    {
        var dict = new Dictionary<string, object>();
        var keyCount = _random.Next(3, 5); // Generate 3-4 key-value pairs for type-level dictionaries

        for (int i = 0; i < keyCount; i++)
        {
            var key = GenerateDictionaryKey(i);
            object value;
            
            // If we have properties analyzed, it means the value type is complex
            if (typeInfo.Properties?.Any() == true)
            {
                // Generate complex object from properties
                value = GenerateObjectFromProperties(typeInfo.Properties);
            }
            else
            {
                // Extract value type from Dictionary<TKey, TValue>
                var valueType = "string"; // Default fallback
                
                if (typeInfo.TypeName.Contains('<') && typeInfo.TypeName.Contains('>'))
                {
                    var start = typeInfo.TypeName.IndexOf('<') + 1;
                    var end = typeInfo.TypeName.LastIndexOf('>');
                    var genericArgs = typeInfo.TypeName.Substring(start, end - start);
                    var types = genericArgs.Split(',');
                    
                    if (types.Length >= 2)
                    {
                        valueType = types[1].Trim(); // Get the value type (second type argument)
                    }
                }
                
                // If value type is not a primitive, create a fallback object structure
                if (!IsPrimitiveType(valueType))
                {
                    value = new
                    {
                        id = _random.Next(1, 1000),
                        name = GenerateStringValue(),
                        value = _random.NextDouble() * 1000,
                        isActive = _random.Next(0, 2) == 1,
                        metadata = new Dictionary<string, object>
                        {
                            ["type"] = valueType,
                            ["generated"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                        }
                    };
                }
                else
                {
                    value = GeneratePrimitiveValue(valueType);
                }
            }
            
            dict[key] = value;
        }

        return dict;
    }

    private object GenerateObjectFromProperties(List<AnalyzedPropertyInfo> properties)
    {
        var obj = new Dictionary<string, object>();

        foreach (var property in properties)
        {
            var value = GenerateValueForProperty(property);
            obj[ToCamelCase(property.Name)] = value;
        }

        return obj;
    }

    private object GenerateValueForProperty(AnalyzedPropertyInfo property)
    {
        // Handle nullable types
        if (property.IsNullable && _random.Next(0, 10) < 2) // 20% chance of null
        {
            return null!;
        }

        // Handle collections
        if (property.IsCollection)
        {
            var itemCount = _random.Next(1, 3);
            var items = new List<object>();

            for (int i = 0; i < itemCount; i++)
            {
                if (property.NestedProperties?.Any() == true)
                {
                    // Generate complex objects for collection elements
                    items.Add(GenerateObjectFromProperties(property.NestedProperties));
                }
                else if (!string.IsNullOrEmpty(property.GenericArgument))
                {
                    items.Add(GeneratePrimitiveValue(property.GenericArgument));
                }
                else
                {
                    items.Add($"Item {i + 1}");
                }
            }

            return items;
        }

        // Handle dictionaries
        if (property.IsDictionary)
        {
            return GenerateDictionaryValue(property);
        }

        // Handle complex nested objects
        if (property.NestedProperties?.Any() == true)
        {
            return GenerateObjectFromProperties(property.NestedProperties);
        }

        // Handle primitive types
        return GeneratePrimitiveValue(property.Type);
    }

    private object GeneratePrimitiveValue(string typeName)
    {
        // Remove nullable indicator
        typeName = typeName.TrimEnd('?');

        return typeName.ToLowerInvariant() switch
        {
            "string" => GenerateStringValue(),
            "int" or "int32" => _random.Next(1, 1000),
            "long" or "int64" => _random.NextInt64(1, 100000),
            "short" or "int16" => (short)_random.Next(1, 1000),
            "byte" => (byte)_random.Next(0, 256),
            "sbyte" => (sbyte)_random.Next(-128, 128),
            "uint" or "uint32" => (uint)_random.Next(1, 1000),
            "ulong" or "uint64" => (ulong)_random.NextInt64(1, 100000),
            "ushort" or "uint16" => (ushort)_random.Next(1, 1000),
            "float" or "single" => (float)(_random.NextDouble() * 1000),
            "double" => _random.NextDouble() * 1000,
            "decimal" => (decimal)(_random.NextDouble() * 1000),
            "bool" or "boolean" => _random.Next(0, 2) == 1,
            "char" => (char)_random.Next(65, 91), // A-Z
            "datetime" => DateTime.UtcNow.AddDays(_random.Next(-365, 365)),
            "dateonly" => DateOnly.FromDateTime(DateTime.UtcNow.AddDays(_random.Next(-365, 365))),
            "timeonly" => TimeOnly.FromDateTime(DateTime.UtcNow),
            "timespan" => TimeSpan.FromMinutes(_random.Next(1, 1440)),
            "guid" => Guid.NewGuid(),
            _ => "Unknown Type" // Default to string for unknown types
        };
    }

    private string GenerateStringValue()
    {
        var sampleStrings = new[]
        {
            "Sample Text", "Example Value", "Test Data", "Mock Content",
            "Generated String", "Random Text", "Sample Value", "Test String",
            "Example Data", "Mock Text", "Sample Content", "Test Value"
        };

        return sampleStrings[_random.Next(sampleStrings.Length)];
    }

    private bool IsPrimitiveType(string typeName)
    {
        var cleanTypeName = typeName.TrimEnd('?').ToLowerInvariant();
        var primitiveTypes = new[]
        {
            "string", "int", "int32", "long", "int64", "short", "int16",
            "byte", "sbyte", "uint", "uint32", "ulong", "uint64", "ushort", "uint16",
            "float", "single", "double", "decimal", "bool", "boolean", "char",
            "datetime", "dateonly", "timeonly", "timespan", "guid", "object"
        };

        return primitiveTypes.Contains(cleanTypeName);
    }

    private object GenerateSimpleFallback(string method)
    {
        return method.ToUpperInvariant() switch
        {
            "GET" => new
            {
                id = 1,
                name = "Sample Item",
                value = "Example Value",
                isActive = true,
                count = _random.Next(1, 100),
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            },
            "POST" => new
            {
                id = _random.Next(100, 999),
                message = "Resource created successfully",
                isSuccess = true,
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            },
            "PUT" => new
            {
                id = 1,
                message = "Resource updated successfully",
                isSuccess = true,
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            },
            _ => new
            {
                message = "Operation completed successfully",
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }
        };
    }

    private string ToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsLower(input[0]))
            return input;

        return char.ToLowerInvariant(input[0]) + input.Substring(1);
    }
}