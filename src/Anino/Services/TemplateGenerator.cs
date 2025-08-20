using System.Text.Json;

namespace Anino.Services;

public class TemplateGenerator : ITemplateGenerator
{
    public void GenerateTemplate(string fileName)
    {
        var template = CreateSampleTemplate();
        var jsonOptions = new JsonSerializerOptions 
        { 
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var jsonContent = JsonSerializer.Serialize(template, jsonOptions);
        File.WriteAllText(fileName, jsonContent);
    }

    private static object[] CreateSampleTemplate()
    {
        return
        [
            new
            {
                path = "/api/users",
                method = "GET",
                statusCode = 200,
                response = new[]
                {
                    new { id = 1, name = "John Doe", email = "john.doe@example.com", role = "admin" },
                    new { id = 2, name = "Jane Smith", email = "jane.smith@example.com", role = "user" },
                    new { id = 3, name = "Bob Johnson", email = "bob.johnson@example.com", role = "user" }
                }
            },
            new
            {
                path = "/api/users/{id}",
                method = "GET",
                statusCode = 200,
                response = new { id = 1, name = "John Doe", email = "john.doe@example.com", role = "admin" }
            },
            new
            {
                path = "/api/users",
                method = "POST",
                statusCode = 201,
                response = new { id = 4, name = "New User", email = "new.user@example.com", role = "user" }
            },
            new
            {
                path = "/api/users/{id}",
                method = "PUT",
                statusCode = 200,
                response = new { id = 1, name = "John Doe Updated", email = "john.doe.updated@example.com", role = "admin" }
            },
            new
            {
                path = "/api/users/{id}",
                method = "DELETE",
                statusCode = 204,
                response = (object?)null
            },
            new
            {
                path = "/api/products",
                method = "GET",
                statusCode = 200,
                response = new[]
                {
                    new { id = 1, name = "Laptop", price = 999.99, category = "Electronics" },
                    new { id = 2, name = "Mouse", price = 29.99, category = "Electronics" },
                    new { id = 3, name = "Keyboard", price = 79.99, category = "Electronics" }
                }
            },
            new
            {
                path = "/api/products/{id}",
                method = "GET",
                statusCode = 200,
                response = new { id = 1, name = "Laptop", price = 999.99, category = "Electronics" }
            },
            new
            {
                path = "/api/health",
                method = "GET",
                statusCode = 200,
                response = new { status = "healthy", timestamp = "2024-01-01T00:00:00Z", version = "1.0.0" }
            },
            new
            {
                path = "/api/error",
                method = "GET",
                statusCode = 500,
                response = new { error = "Internal Server Error", message = "Something went wrong" }
            }
        ];
    }
}