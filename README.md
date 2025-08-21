# Anino - Mock API Server

Anino is a lightweight, fast mock API server that allows you to quickly spin up REST API endpoints from a simple JSON definition file. Perfect for frontend development, testing, and prototyping when you need a backend API that doesn't exist yet.

## Features

- 🚀 **Fast Setup** - Define all your API endpoints in a single JSON file.
- 🔧 **Nested Subcommand Interface** - Clean and clear CLI with `server` and `def new` commands.
- ⚡ **Latency Simulation** - Add artificial delays to simulate real network conditions.
- 🎯 **Custom Status Codes** - Return any HTTP status code you need.
- 📝 **Rich Definition Generator** - Quickly generate a sample definition file with common operations.
- 🛠️ **Cross-Platform** - Runs on Windows, macOS, and Linux.
- 📦 **.NET Global Tool** - Install once, use anywhere.

## Installation

Install Anino as a .NET global tool:

```bash
dotnet tool install -g Anino.Tool
```

### Prerequisites

- .NET 8.0 or .NET 9.0 runtime is required.
- You can check your installed runtimes with `dotnet --list-runtimes`.
- Download .NET from: https://dotnet.microsoft.com/download

## Quick Start

1.  **Generate a sample definition file:**
    The `def new` command creates a `definition.json` file in your current directory.
    ```bash
    anino def new
    ```

2.  **Start the mock server:**
    The `server` command starts the API server using the definition file.
    ```bash
    anino server --def definition.json
    ```

3.  **Test your new API:**
    Open a new terminal and use `curl` or any API client to test the endpoints.
    ```bash
    curl http://localhost:5000/api/users
    curl http://localhost:5000/api/health
    ```

## Command Line Reference

### `anino server`

Starts the Anino mock API server using a JSON definition file.

| Option             | Alias | Description                                      | Required | Default |
| ------------------ | ----- | ------------------------------------------------ | -------- | ------- |
| `--def`            | `-d`  | Path to the JSON definition file.                | Yes      |         |
| `--port`           | `-p`  | The port for the server to listen on.            | No       | `5000`  |
| `--latency`        | `-l`  | Simulated network latency in milliseconds.       | No       | `0`     |

**Examples:**

```bash
# Start server with a required definition file
anino server --def ./api/definitions.json

# Start server on a different port with an alias
anino server -d definitions.json -p 8080

# Start server with 500ms latency
anino server -d definitions.json -l 500
```

### `anino def new`

Creates a new sample API definition file. This is a subcommand of `def`.

| Option             | Alias | Description                                      | Required | Default          |
| ------------------ | ----- | ------------------------------------------------ | -------- | ---------------- |
| `--name`           | `-n`  | The name of the definition file to be created.   | No       | `definition.json` |

**Examples:**

```bash
# Generate a default definition file named definition.json
anino def new

# Generate a definition file with a custom name
anino def new --name my-api
```

## Sample Definition File

The `anino def new` command creates a file with a structure like this, which you can customize for your needs.

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
        "email": "john.doe@example.com"
      }
    ]
  },
  {
    "path": "/api/health",
    "method": "GET",
    "statusCode": 200,
    "response": {
      "status": "healthy",
      "timestamp": "2025-08-21T12:00:00Z"
    }
  }
]
```
