using Microsoft.Agents.M365Copilot.Beta;
using Microsoft.Agents.M365Copilot.Beta.Copilot.Retrieval;
using Microsoft.Agents.M365Copilot.Beta.Models;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace M365.CoPilot.MCP;

[McpServerToolType]
public static class CoPilotTool
{
    [McpServerTool(Name = "Retrieve")]
    [Description("Retrieves relevant knowledge information from internal systems (Confluence, Jira, Azure Devops, Salesforce...) in a secure and compliant way.")]
    public static async Task<string> RetrieveAsync(AgentsM365CopilotBetaServiceClient client, IOptions<CoPilotMcpOptions> options,
        [Description("Natural language query string used to retrieve relevant text extracts. This parameter has a limit of 1,500 characters. " +
        "Your query should be a single sentence, and you should avoid spelling errors in context-rich keywords.")]
        string query,
        [Description("The data source from which to retrieve the extracts (sharePoint, oneDriveBusiness, externalItem). If not specified, the default is all available sources.")]
        RetrievalDataSource? dataSource = null,
        [Description("Indicates whether extracts should be retrieved from specific connectors (if using externalItem).")]
        string[]? connectionIds = null,
        [Description("The number of results that are returned in the response. Must be between 1 and 25. By default, returns up to 25 results.")]
        int maximumNumberOfResults = 25)
    {
        var body = new RetrievalPostRequestBody
        {
            QueryString = query,
            DataSource = dataSource ?? options.Value.Scopes switch
            {
                var scopes when scopes.Contains("ExternalItem.Read.All") && !scopes.Contains("Sites.Read.All") && !scopes.Contains("Files.Read.All") =>
                    RetrievalDataSource.ExternalItem,
                _ => null
            },
            MaximumNumberOfResults = maximumNumberOfResults,
        };
        if (connectionIds != null && connectionIds.Length > 0)
        {
            body.DataSourceConfiguration = new DataSourceConfiguration
            {
                ExternalItem = new ExternalItemConfiguration
                {
                    Connections = [.. connectionIds.Select(id => new ConnectionItem() { ConnectionId = id })],
                },
            };
        }

        var result = await client.Copilot.Retrieval.PostAsync(body);
        return result?.RetrievalHits == null ?
            "No retrieval hits found in the response" :
            JsonSerializer.Serialize(result.RetrievalHits);
    }
}
