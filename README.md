# Anino - Mock API Server

Anino is a lightweight, fast mock API server that allows you to quickly spin up REST API endpoints from a simple JSON configuration file. Perfect for frontend development, testing, and prototyping when you need a backend API that doesn't exist yet.

## Features

- 🚀 **Fast Setup** - Define API endpoints in a simple JSON file
- 🔧 **Flexible Configuration** - Support for all HTTP methods (GET, POST, PUT, DELETE, etc.)
- ⚡ **Latency Simulation** - Add artificial delays to simulate real network conditions
- 🎯 **Custom Status Codes** - Return any HTTP status code you need
- 📝 **Template Generator** - Generate sample configuration files with common CRUD operations
- 🛠️ **Cross-Platform** - Runs on Windows, macOS, and Linux
- 📦 **Global Tool** - Install once, use anywhere

## Installation

Install Anino as a global .NET tool:

```bash
dotnet tool install -g Anino.Tool
```

### Prerequisites

- .NET 8.0 or .NET 9.0 runtime
- Check your installed runtimes: `dotnet --list-runtimes`
- Download .NET from: https://dotnet.microsoft.com/download

## Quick Start

1. **Generate a sample configuration file:**
   ```bash
   anino --generate-template
   ```

2. **Start the mock server:**
   ```bash
   anino --file template.json
   ```

3. **Test your API:**
   ```bash
   curl http://localhost:5000/api/users
   ```

## Configuration File Format

Create a JSON file that defines your API endpoints:

```json
[
  {
    "path": "/api/users",
    "method": "GET",
    "statusCode": 200,
    "response": [
      {
        "id": 1,
        "name": "John Doe",
        "email": "john@example.com"
      },
      {
        "id": 2,
        "name": "Jane Smith",
        "email": "jane@example.com"
      }
    ]
  },
  {
    "path": "/api/users",
    "method": "POST",
    "statusCode": 201,
    "response": {
      "id": 3,
      "name": "New User",
      "email": "newuser@example.com"
    }
  },
  {
    "path": "/api/users/{id}",
    "method": "GET",
    "statusCode": 200,
    "response": {
      "id": 1,
      "name": "John Doe",
      "email": "john@example.com"
    }
  },
  {
    "path": "/api/users/{id}",
    "method": "DELETE",
    "statusCode": 204,
    "response": {}
  }
]
```

### Configuration Properties

- **`path`** - The API endpoint path (supports route parameters like `{id}`)
- **`method`** - HTTP method (GET, POST, PUT, DELETE, PATCH, etc.)
- **`statusCode`** - HTTP status code to return (200, 201, 404, 500, etc.)
- **`response`** - JSON response body (can be object, array, or primitive)

## Command Line Options

```bash
anino [options]
```

### Options

- `--file <path>` - **Required**. Path to the JSON configuration file
- `--port <number>` - Server port (default: 5000)
- `--latency <milliseconds>` - Add artificial network latency to all responses
- `--generate-template [filename]` - Generate a sample template file (default: template.json)

### Examples

**Start server with custom port:**
```bash
anino --file api-config.json --port 3000
```

**Add 500ms latency to simulate slow network:**
```bash
anino --file api-config.json --latency 500
```

**Generate template with custom filename:**
```bash
anino --generate-template my-api-template.json
```

## Use Cases

### Frontend Development
Mock backend APIs while developing your frontend application:
```bash
anino --file user-api.json --port 8080
```

### Testing
Create predictable API responses for your integration tests:
```bash
anino --file test-fixtures.json --port 9000 --latency 100
```

### Prototyping
Quickly prototype API designs before implementing the real backend:
```bash
anino --file prototype-api.json
```

### Demo Preparation
Create reliable demo data that won't change during presentations:
```bash
anino --file demo-data.json --port 5000
```

## Advanced Configuration Examples

### Error Responses
```json
[
  {
    "path": "/api/users/{id}",
    "method": "GET",
    "statusCode": 200,
    "response": {
      "user": null,
      "message": "No user found with the specified ID"
    }
  },
  {
    "path": "/api/users",
    "method": "POST",
    "statusCode": 400,
    "response": {
      "error": "Validation failed",
      "details": [
        "Email is required",
        "Name must be at least 2 characters"
      ]
    }
  },
  {
    "path": "/api/nonexistent-endpoint",
    "method": "GET",
    "statusCode": 404,
    "response": {
      "error": "Not Found",
      "message": "The requested endpoint does not exist"
    }
  }
]
```

### Complex Nested Data
```json
[
  {
    "path": "/api/orders",
    "method": "GET",
    "statusCode": 200,
    "response": {
      "data": [
        {
          "id": 1,
          "customerId": 101,
          "items": [
            {
              "productId": 201,
              "name": "Laptop",
              "quantity": 1,
              "price": 999.99
            }
          ],
          "total": 999.99,
          "status": "shipped",
          "createdAt": "2024-01-15T10:30:00Z"
        }
      ],
      "pagination": {
        "page": 1,
        "limit": 10,
        "total": 1,
        "totalPages": 1
      }
    }
  }
]
```

## Development

### Prerequisites
- .NET 8.0 SDK or later

### Building from Source
```bash
git clone <repository-url>
cd anino
dotnet build
```

### Running Tests
```bash
dotnet test
```

### Creating a Release Package
```bash
dotnet pack --configuration Release
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

If you encounter any issues or have questions:

1. Check the existing issues in the repository
2. Create a new issue with a detailed description
3. Include your configuration file and command line arguments if relevant

---

**Happy mocking!** 🎭
