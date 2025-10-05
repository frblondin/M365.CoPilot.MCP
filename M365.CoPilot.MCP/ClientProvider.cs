using Azure.Core;
using Azure.Identity;
using Azure.Identity.Broker;
using Microsoft.Agents.M365Copilot.Beta;
using System.Runtime.InteropServices;

namespace M365.CoPilot.MCP;

internal static partial class ClientProvider
{
    private const string TokenCacheName = "TokenCache";

    internal static async Task<AgentsM365CopilotBetaServiceClient> GetClientAsync(string tenantId, string clientId, string[] scopes)
    {
        var credential = await CreateCredentialAsync(tenantId, clientId, scopes);
        return new AgentsM365CopilotBetaServiceClient(credential, scopes);
    }

    private static async Task<TokenCredential> CreateCredentialAsync(string tenantId, string clientId, string[] scopes)
    {
        if (File.Exists(TokenCacheName))
        {
            try
            {
                using var reader = File.OpenRead(TokenCacheName);
                var newRecord = await AuthenticationRecord.DeserializeAsync(reader);
                return new InteractiveBrowserCredential(CreateOptions(tenantId, clientId, newRecord));
            }
            catch
            {
                // If the cache is invalid or deserialization fails, fall back to creating a new credential and cache.
                // This exception can be safely ignored as a new authentication flow will be triggered.
            }
        }
        var result = new InteractiveBrowserCredential(CreateOptions(tenantId, clientId));
        var record = await result.AuthenticateAsync(new TokenRequestContext(scopes));
        using var writer = File.Create(TokenCacheName);
        await record.SerializeAsync(writer);
        return result;
    }

    private static InteractiveBrowserCredentialOptions CreateOptions(string tenantId, string clientId, AuthenticationRecord? record = null)
    {
        var handle = GetConsoleWindowHandle();
        var result = handle.HasValue ?
            new InteractiveBrowserCredentialBrokerOptions(handle.Value) :
            new InteractiveBrowserCredentialOptions();
        result.TenantId = tenantId;
        result.ClientId = clientId;
        result.RedirectUri = new Uri("http://localhost");
        result.TokenCachePersistenceOptions = new TokenCachePersistenceOptions
        {
            Name = TokenCacheName,
        };
        if (record != null)
        {
            result.AuthenticationRecord = record;
        }
        return result;
    }

    private static nint? GetConsoleWindowHandle()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var handle = GetConsoleWindow();
            return handle.ToInt64() != 0L ? handle : null;
        }
        return null;
    }

    [LibraryImport("kernel32.dll")]
    private static partial IntPtr GetConsoleWindow();
}
