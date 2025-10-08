using System.CommandLine;
using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace M365.CoPilot.MCP;

public class CoPilotMcpOptions { public string[]? Scopes { get; set; } }

static class Program
{
    static async Task<int> Main(string[] args)
    {
        return await CreateRootCommand().InvokeAsync(args);
    }

    private static async Task RunMcpServerAsync(Guid tenantId, Guid clientId, string[] scopes)
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Logging.AddConsole(consoleLogOptions =>
        {
            // Configure all logs to go to stderr
            consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
        });

        var client = await ClientProvider.GetClientAsync(tenantId.ToString(), clientId.ToString(), scopes.ToArray());

        builder.Services
            .AddSingleton(client)
            .Configure<CoPilotMcpOptions>(o => o.Scopes = scopes)
            .AddMcpServer()
            .WithPrompts<CoPilotPrompts>()
            .WithStdioServerTransport()
            .WithToolsFromAssembly();

        await builder.Build().RunAsync();
    }

    private static RootCommand CreateRootCommand()
    {
        var tenantIdOption = new Option<Guid>(
            "--tenant-id", "The Azure AD tenant ID")
        {
            IsRequired = true,
        };

        var clientIdOption = new Option<Guid>(
            "--client-id", "The Azure AD application client ID")
        {
            IsRequired = true,
        };

        var scopesOption = new Option<string[]>(
            "--scopes",
            "List of scopes for the authentication context. Scopes depend on the data sources you want to use in the Retrieve method: " +
            "ExternalItem.Read.All for externalItem, Files.Read.All & Sites.Read.All for sharePoint & oneDriveBusiness)")
        {
            IsRequired = true,
            AllowMultipleArgumentsPerToken = true,
        };
        scopesOption.AddValidator(ValidateScopes);

        var rootCommand = new RootCommand("Microsoft Graph MCP Server")
        {
            tenantIdOption,
            clientIdOption,
            scopesOption,
        };

        rootCommand.SetHandler(RunMcpServerAsync, tenantIdOption, clientIdOption, scopesOption);
        return rootCommand;
    }

    private static void ValidateScopes(OptionResult result)
    {
        var values = result.GetValueOrDefault<string[]>() ?? [];
        if (values == null || values.Length == 0)
        {
            result.ErrorMessage = "At least one scope must be provided.";
        }
        var acceptedValues = new HashSet<string>
            {
                "ExternalItem.Read.All",
                "Files.Read.All",
                "Sites.Read.All"
            };
        foreach (var value in values)
        {
            if (!acceptedValues.Contains(value))
            {
                result.ErrorMessage = $"Invalid scope '{value}'. Accepted scopes are: {string.Join(", ", acceptedValues)}.";
                break;
            }
        }
        if (values.Contains("Sites.Read.All") != values.Contains("Files.Read.All"))
        {
            result.ErrorMessage = "If using Sites.Read.All or Files.Read.All, both scopes must be specified.";
        }
    }
}