using System.Collections.Generic;
using System.Linq;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.CI.GitHubActions.Configuration;
using Nuke.Common.Execution;
using Nuke.Common.Utilities;

public class GitHubActionsCustomAttribute : GitHubActionsAttribute
{
    public required string DotNetVersion { get; init; }

    public GitHubActionsCustomAttribute(string name, GitHubActionsImage image, params GitHubActionsImage[] images)
        : base(name, image, images)
    {
    }

    protected override GitHubActionsJob GetJobs(GitHubActionsImage image, IReadOnlyCollection<ExecutableTarget> relevantTargets)
    {
        var result = base.GetJobs(image, relevantTargets);
        var setupDotNet = new GitHubActionsSetupDotNetStep
        {
            Version = DotNetVersion,
        };
        result.Steps = [.. result.Steps.Prepend(setupDotNet)];

        return result;
    }
}

public class GitHubActionsSetupDotNetStep : GitHubActionsStep
{
    public string Version { get; set; }

    public override void Write(CustomFileWriter writer)
    {
        writer.WriteLine("- uses: actions/setup-dotnet@v5");

        using (writer.Indent())
        {
            writer.WriteLine("with:");
            using (writer.Indent())
            {
                writer.WriteLine($"dotnet-version: '{Version}'");
            }
        }
    }
}

