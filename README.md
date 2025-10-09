# M365 CoPilot MCP Server

![NuGet Version](https://img.shields.io/nuget/vpre/M365.CoPilot.MCP)
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
- **Pre-built Prompts**: 10 specialized prompts for common organizational knowledge scenarios

## 📋 Prerequisites

- **.NET 10 SDK** or later: [Download .NET](https://dotnet.microsoft.com/en-us/download)
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
    "m365-copilot": {
      "command": "dnx",
      "args": [
         "--yes", "--prerelease", "M365.CoPilot.MCP",
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
    "m365-copilot-dev": {
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

## 📋 Pre-built Prompts

The MCP server includes 10 specialized prompts designed for common organizational knowledge scenarios. These prompts provide structured search queries and response formatting to help you quickly find relevant information.

### 🔬 Research & Development

#### `project_research`
Research project documentation, best practices, and technical resources across your organization's knowledge base.
- **Use for**: Development standards, architecture decisions, project-related documentation
- **Example**: `#project_research "mobile app development"`

#### `code_standards`
Access coding standards, architecture guidelines, and technical best practices from your organization.
- **Use for**: Development conventions, architectural decisions, style guides
- **Example**: `#code_standards "React development"`

#### `integration_patterns`
Find integration patterns, API documentation, and system integration guides from your technical resources.
- **Use for**: System connections, data flows, API implementations
- **Example**: `#integration_patterns "payment system integration"`

### 🔧 Operations & DevOps

#### `troubleshooting_guide`
Find troubleshooting guides, error solutions, and technical documentation for resolving common issues.
- **Use for**: Debugging procedures, error resolution, problem-solving steps
- **Example**: `#troubleshooting_guide "authentication errors"`

#### `devops_procedures`
Access DevOps procedures, deployment guides, and infrastructure documentation.
- **Use for**: CI/CD pipelines, deployment strategies, operational procedures
- **Example**: `#devops_procedures "container deployment"`

### 🏢 Business & Compliance

#### `business_process`
Retrieve business process documentation, workflows, and operational procedures from your organization.
- **Use for**: Company processes, compliance requirements, workflow documentation
- **Example**: `#business_process "customer onboarding"`

#### `compliance_documentation`
Find compliance procedures, audit documentation, and regulatory guidelines from your organization.
- **Use for**: Security requirements, privacy policies, regulatory compliance
- **Example**: `#compliance_documentation "GDPR compliance"`

### 📊 Intelligence & Analytics

#### `competitive_intelligence`
Research market analysis, competitor information, and business intelligence from internal reports.
- **Use for**: Market insights, competitive positioning, business strategy
- **Example**: `#competitive_intelligence "mobile payment market"`

#### `customer_insights`
Research customer feedback, survey results, and user research findings from your knowledge base.
- **Use for**: Customer satisfaction, user experience, product feedback analysis
- **Example**: `#customer_insights "user experience feedback"`

### 📚 Learning & Development

#### `training_resources`
Access training materials, onboarding guides, and learning resources from your organization.
- **Use for**: Educational content, skill development, certification programs
- **Example**: `#training_resources "cloud platform training"`

### 🎯 How to Use Prompts

Each prompt generates a structured search query and provides formatting guidance for responses. When you use a prompt:

1. **Execute the prompt**: `#prompt_name "your specific topic"`
2. **Review the generated query**: The prompt will show you the search query it created
3. **Run the search**: Use the provided `#Retrieve` command to execute the search
4. **Get formatted results**: Results will be structured according to the prompt's template

**Example workflow**:
```
> #project_research "API design patterns"
📋 Search Query: `project documentation best practices guidelines standards for API design patterns`

To execute this search, use:
#Retrieve "project documentation best practices guidelines standards for API design patterns"
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
