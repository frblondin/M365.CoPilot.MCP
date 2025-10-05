using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.NerdbankGitVersioning;
using Nuke.Common.Utilities.Collections;
using Octokit;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[ShutdownDotNetAfterServerBuild]
[GitHubActionsCustom(
    "CI",
    GitHubActionsImage.UbuntuLatest,
    DotNetVersion = "10.0.x",
    OnPushBranches =
    [
        "main",
        "dev",
        "releases/**",
    ],
    OnPullRequestBranches =
    [
        "main",
        "releases/**",
    ],

    InvokedTargets = [nameof(Pack)],
    ImportSecrets = ["GITHUB_TOKEN", nameof(NuGetApiKey)],
    WritePermissions = [GitHubActionsPermissions.Packages, GitHubActionsPermissions.Contents],
    FetchDepth = 0)]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Pack);

    [NerdbankGitVersioning(UpdateBuildNumber = true)] readonly NerdbankGitVersioning GitVersion;
    [GitRepository] readonly GitRepository Repository;
    [Solution(SuppressBuildProjectCheck = true)] readonly Solution Solution;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    static AbsolutePath SourceDirectory => RootDirectory / "src";
    static AbsolutePath OutputDirectory => RootDirectory / "output";
    static AbsolutePath NugetDirectory => OutputDirectory / "nuget";

    [Parameter] string NuGetFeed { get; } = "https://api.nuget.org/v3/index.json";
    [Parameter, Secret] readonly string NuGetApiKey;
    [Parameter] string ArtifactsType { get; } = "*.nupkg";
    [Parameter] string ExcludedArtifactsType { get; } = "symbols.nupkg";

    Target Clean => _ => _
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(d => d.DeleteDirectory());
            OutputDirectory.CreateOrCleanDirectory();
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });


    Target Pack => _ => _
        .DependsOn(Compile)
        .Produces(NugetDirectory / ArtifactsType)
        .Triggers(PublishToNuGet, CreateRelease)
        .Executes(() =>
        {
            Solution.AllProjects
                .Where(p => p.GetProperty<string>("PackageType") == "DotnetTool")
                .ForEach(project =>
                    DotNetPack(s => s
                        .SetProject(project)
                        .SetConfiguration(Configuration)
                        .SetVersion(GitVersion.NuGetPackageVersion)
                        .EnableNoBuild()
                        .EnableNoRestore()
                        .SetOutputDirectory(NugetDirectory)));
        });

    Target PublishToNuGet => _ => _
       .Description($"Publishing to NuGet with the version.")
       .Requires(() => Configuration.Equals(Configuration.Release))
       .OnlyWhenStatic(() => GitHubActions.Instance != null &&
                             Repository.IsOnMainOrMasterBranch())
       .Executes(() =>
       {
           NugetDirectory.GlobFiles(ArtifactsType)
               .Where(x => !x.Name.EndsWith(ExcludedArtifactsType))
               .ForEach(x =>
               {
                   DotNetNuGetPush(s => s
                       .SetTargetPath(x)
                       .SetSource(NuGetFeed)
                       .SetApiKey(NuGetApiKey)
                       .EnableSkipDuplicate()
                   );
               });
       });

    Target CreateRelease => _ => _
       .Description($"Creating release for the publishable version.")
       .Requires(() => Configuration.Equals(Configuration.Release))
       .OnlyWhenStatic(() => GitHubActions.Instance != null &&
                             Repository.IsOnMainOrMasterBranch())
       .Executes(async () =>
       {
           GitHubTasks.GitHubClient = new GitHubClient(
               new ProductHeaderValue(nameof(NukeBuild)),
               new Octokit.Internal.InMemoryCredentialStore(
                   new Credentials(GitHubActions.Instance.Token)));

           var (owner, name) = (Repository.GetGitHubOwner(), Repository.GetGitHubName());

           var releaseTag = GitVersion.NuGetPackageVersion;
           var messages = GitChangeLogTasks.CommitsSinceLastTag();
           var latestChangeLog = string.Join("\n", messages.Where(IsReleaseNoteCommit).Select(TurnIntoLog));

           var newRelease = new NewRelease(releaseTag)
           {
               TargetCommitish = Repository.Commit,
               Draft = true,
               Name = $"v{releaseTag}",
               Prerelease = !(Repository.IsOnMainOrMasterBranch() || Repository.IsOnReleaseBranch()),
               Body = latestChangeLog
           };

           var createdRelease = await GitHubTasks.GitHubClient
              .Repository
              .Release.Create(owner, name, newRelease);

           NugetDirectory.GlobFiles(ArtifactsType)
              .Where(x => !x.Name.EndsWith(ExcludedArtifactsType))
              .ForEach(async x => await UploadReleaseAssetToGitHub(createdRelease, x));

           await GitHubTasks.GitHubClient
              .Repository.Release
              .Edit(owner, name, createdRelease.Id, new ReleaseUpdate { Draft = false });

           static bool IsReleaseNoteCommit(string message) =>
               !message.Contains("[skip release notes]", StringComparison.OrdinalIgnoreCase);

           static string TurnIntoLog(string message) =>
               $"- {Regex.Replace(message, @"\s*\[.*\]", string.Empty)}";

            static async Task UploadReleaseAssetToGitHub(Release release, string asset)
            {
                await using var artifactStream = System.IO.File.OpenRead(asset);
                var fileName = Path.GetFileName(asset);
                var assetUpload = new ReleaseAssetUpload
                {
                    FileName = fileName,
                    ContentType = "application/octet-stream",
                    RawData = artifactStream,
                };
                await GitHubTasks.GitHubClient.Repository.Release.UploadAsset(release, assetUpload);
            }
       });
}
