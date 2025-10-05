# M365 CoPilot MCP Server

[![NuGet](https://img.shields.io/badge/NuGet-004880?logo=nuget&logoColor=fff)](https://www.nuget.org/packages/M365.CoPilot.MCP)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
<img src="https://upload.wikimedia.org/wikipedia/commons/0/0e/Model_Context_Protocol_logo.png" width="19" alt="MCP"/>

A **Model Context Protocol (MCP) Server** that provides secure and compliant access to Microsoft 365 knowledge systems through Microsoft Graph and Copilot Retrieval APIs. This server enables AI assistants to retrieve relevant information from internal systems like Confluence, Jira, Azure DevOps, Salesforce, and other connected data sources.

## 🚀 Features

- **Secure Knowledge Retrieval**: Access internal organizational knowledge through Microsoft Graph
- **Multi-Source Integration**: Retrieve data from Confluence, Jira, Azure DevOps, Salesforce, and more
- **MCP Compliance**: Implements the Model Context Protocol for seamless AI assistant integration
- **Azure AD Authentication**: Secure authentication with support for interactive browser and token caching
- **Configurable Results**: Customize the number of results and target specific data connectors
- **Cross-Platform**: Built on .NET 10 with cross-platform support

## 📋 Prerequisites

- **.NET 10 SDK** or later
- **Azure AD Application** with appropriate permissions
- **Microsoft 365 Copilot license** (for retrieval functionality)
- **Windows, macOS, or Linux** environment

## ⚙️ Installation & Configuration

### Entra App Registration (Azure AD administrator, once)

1. Register an application in the [Azure Portal](https://portal.azure.com/)
2. Add a Redirect URI: `http://localhost`
3. Add the following API permissions:
   - `ExternalItem.Read.All` (Microsoft Graph) - Application: for accessing connected data sources
   - `Files.Read.All` and `Sites.Read.All` (Microsoft Graph) - Application: for accessing SharePoint and OneDrive data

### GitHub Copilot MCP Configuration

To use this MCP server with GitHub Copilot, configure your MCP settings file:

#### Normal Usage

- Edit your list of MCP servers. For vscode, run the **MCP: Open User Configuration** command which opens the mcp.json file in your user profile. If the file does not exist, VS Code creates it for you

```json
{
  "mcpServers": {
    "microsoft-graph-dev": {
      "command": "dnx",
      "args": [
         "--prerelease", "M365.CoPilot.MCP",
         "--tenant-id", "your-dev-tenant-id",
         "--client-id", "your-dev-client-id",
         "--scopes", "Files.Read.All Sites.Read.All ExternalItem.Read.All"
      ]
    }
  }
}
```

#### Using Local Build (Development)
This allows you to test changes immediately without publishing or installing packages.
```json
{
  "mcpServers": {
    "microsoft-graph-dev": {
      "command": "C:\\path\\to\\your\\repo\\M365.CoPilot.MCP\\M365.CoPilot.MCP\\bin\\Debug\\net10.0\\M365.CoPilot.MCP.exe",
      "args": [
         "--tenant-id", "your-dev-tenant-id",
         "--client-id", "your-dev-client-id",
         "--scopes", "Files.Read.All Sites.Read.All ExternalItem.Read.All"
      ]
    }
  }
}
```

### Command Line

| Parameter | Description | Example |
|-----------|-------------|---------------|
| `--tenant-id` | Azure AD tenant ID | `7DCD55F6-A3D3-43B0-BCE9-75BAD16B107B` |
| `--client-id` | Azure AD application client ID | `A681D4ED-5970-4999-BE77-B38F49296ED3` |
| `--scopes` | List of scopes for the authentication context. Scopes depend on the data sources you want to use in the Retrieve method: ExternalItem.Read.All for externalItem, Files.Read.All & Sites.Read.All for sharePoint & oneDriveBusiness) | `Files.Read.All Sites.Read.All ExternalItem.Read.All` |

## 🔍 Available Tools

### Retrieve Tool

The primary tool provided by this MCP server:

**Name**: `Retrieve`

**Description**: Retrieves relevant knowledge information from internal systems in a secure and compliant way.

**Parameters**:
- `query` (required): Natural language query string used to retrieve relevant text extracts (max 1,500 characters). Your query should be a single sentence, and you should avoid spelling errors in context-rich keywords.
- `dataSource` (optional): The data source from which to retrieve the extracts (sharePoint, oneDriveBusiness, externalItem). If not specified, the default is all available sources.
- `connectionIds` (optional): Array of specific connector IDs to target (used when dataSource is externalItem)
- `maximumNumberOfResults` (optional): Number of results to return (1-25, default: 25)

**Example Query (simple)**:
```json
{
  "query": "project management best practices for agile development"
}
```

**Example Query (advanced)**:
```json
{
  "query": "project management best practices for agile development",
  "dataSource": "externalItem",
  "connectionIds": ["confluence-connector"],
  "maximumNumberOfResults": 10
}
```

## 🏗️ Architecture

```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────────┐
│   AI Assistant  │ ─> │   MCP Server     │ ─> │  Microsoft Graph    │
│                 │    │  (This Project)  │    │  Copilot Retrieval  │
└─────────────────┘    └──────────────────┘    └─────────────────────┘
                                │                          │
                                v                          v
                       ┌─────────────────┐    ┌─────────────────────┐
                       │ Token Caching   │    │  Data Connectors    │
                       │ & Auth Flow     │    │  • Confluence       │
                       └─────────────────┘    │  • Jira             │
                                              │  • Azure DevOps     │
                                              │  • Salesforce       │
                                              │  • ...              │
                                              └─────────────────────┘
```

## 🔒 Security & Authentication

- **Interactive Browser Authentication**: Secure OAuth 2.0 flow with Azure AD
- **Token Caching**: Persistent token storage for improved user experience
- **Windows Authentication Broker**: Enhanced security on Windows platforms
- **Scope-Based Access**: Requests only necessary permissions (`ExternalItem.Read.All`)

## 🛠️ Development

### Key Dependencies

- **ModelContextProtocol**: MCP server framework
- **Microsoft.Agents.M365Copilot.Beta**: Microsoft 365 Copilot SDK
- **Azure.Identity**: Azure authentication
- **System.CommandLine**: CLI argument parsing
- **Microsoft.Extensions.Hosting**: Application hosting framework

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔗 Related Resources

- [Model Context Protocol Documentation](https://modelcontextprotocol.io/)
- [Microsoft Graph Documentation](https://docs.microsoft.com/en-us/graph/)
- [Microsoft 365 Copilot Development](https://docs.microsoft.com/en-us/copilot/)
- [Azure Identity Documentation](https://docs.microsoft.com/en-us/dotnet/api/azure.identity)
- [GitHub Copilot MCP Configuration](https://docs.github.com/en/copilot)

## 📞 Support

For support and questions:

- **Issues**: [GitHub Issues](https://github.com/your-org/Microsoft.Graph.MCP/issues)
- **Discussions**: [GitHub Discussions](https://github.com/your-org/Microsoft.Graph.MCP/discussions)
- **Documentation**: [Wiki](https://github.com/your-org/Microsoft.Graph.MCP/wiki)
