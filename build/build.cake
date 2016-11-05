#addin "MagicChunks"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var paths = new
{
    BasePath = Directory("./../"),
    Sources =  Directory("./../sources/"),
    Artifacts = new
    {
        Sources = Directory("./../artifacts/sources/"),
        Build = Directory("./../artifacts/bin/"),
        Nuget = Directory("./../artifacts/nuget/"),
        MainProject = Directory("./../artifacts/sources/") + Directory("CqrsSpirit"),
        TestsProject = Directory("./../artifacts/sources/") + Directory("CqrsSpirit.Tests"),
    }
};

var buildTag = Argument("buildtag", EnvironmentVariable("APPVEYOR_REPO_TAG") ?? EnvironmentVariable("APPVEYOR_BUILD_VERSION") ?? "1.0.0");
var buildNumber = Argument("buildnumber", EnvironmentVariable("APPVEYOR_BUILD_NUMBER") ?? "0");
var isRelease = string.IsNullOrWhiteSpace(EnvironmentVariable("APPVEYOR_REPO_TAG")) == false;

Task("Prepare-Artifacts")
    .Does(() =>
{
    EnsureDirectoryExists(paths.Artifacts.Sources);
    EnsureDirectoryExists(paths.Artifacts.Build);
    EnsureDirectoryExists(paths.Artifacts.Nuget);

    CleanDirectory(paths.Artifacts.Sources);
    CleanDirectory(paths.Artifacts.Build);
    CleanDirectory(paths.Artifacts.Nuget);

    CopyDirectory(paths.Sources, paths.Artifacts.Sources);
});

Task("Restore-Packages")
    .IsDependentOn("Prepare-Artifacts")
    .Does(() =>
{
    DotNetCoreRestore(paths.Artifacts.Sources);
});

Task("Update-Version")
    .IsDependentOn("Prepare-Artifacts")
    .Does(() =>
{
    Information("APPVEYOR_REPO_TAG: {0}", EnvironmentVariable("APPVEYOR_REPO_TAG"));
    Information("APPVEYOR_BUILD_VERSION: {0}", EnvironmentVariable("APPVEYOR_BUILD_VERSION"));
    Information("APPVEYOR_BUILD_NUMBER: {0}", EnvironmentVariable("APPVEYOR_BUILD_NUMBER"));

    var match = System.Text.RegularExpressions.Regex.Match(buildTag, @"\d+\.\d+\.\d+");
    var version = match.Success ? match.Value : "1.0.0";

    var projectVersion = isRelease ? String.Format("{0}.{1}", version, buildNumber) : String.Format("{0}-dev{1}", version, buildNumber);
    Information("Project Version: {0}", projectVersion);

    TransformConfig(paths.Artifacts.MainProject + File("project.json"),
        new TransformationCollection {{ "version", projectVersion }});
});

Task("Build")
    .IsDependentOn("Prepare-Artifacts")
    .IsDependentOn("Restore-Packages")
    .IsDependentOn("Update-Version")
    .Does(() =>
{
    DotNetCoreBuild(paths.Artifacts.MainProject, new DotNetCoreBuildSettings
    {
        Framework = "netstandard1.6",
        Configuration = configuration,
        OutputDirectory = paths.Artifacts.Build
    });
});

Task("Run-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreTest(paths.Artifacts.TestsProject);
});

Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCorePack(paths.Artifacts.MainProject, new DotNetCorePackSettings
    {
        Configuration = configuration,
        OutputDirectory = paths.Artifacts.Nuget
    });
});

Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Tests")
    .IsDependentOn("Pack");

RunTarget(target);
