#tool "nuget:?package=NUnit.ConsoleRunner"
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=ReportGenerator"
#tool "nuget:?package=ReportUnit"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

var solutionPath = "";
var pathCoverage = File("./coverage.xml");
var tempDir = Directory(System.IO.Path.GetTempPath());

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Detect-Solution")
.Does(() => {
    var files = GetFiles("./*.sln");
    if (files.Count == 0) {
        throw new FileNotFoundException("Not found solution");
    }

    var solutionName = files.First().GetFilename().FullPath;
    solutionPath = System.IO.Path.Combine(".", solutionName);
    
    Information("Solution Path: " + solutionPath);
});

Task("Clean")
    .IsDependentOn("Detect-Solution")
    .Does(() =>
{
    MSBuild(solutionPath, settings =>
        settings.SetConfiguration(configuration)
        .WithTarget("Clean"));
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionPath);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild(solutionPath, settings =>
        settings.SetConfiguration(configuration));
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var testLibraries = GetFiles("./**/bin/" + configuration + "/*.Tests.dll")
                        .Select(test => test.FullPath);
    
    OpenCover(tool => {
        tool.NUnit3("./WcfService.Tests/bin/" + configuration +"/WcfService.Tests.dll",
            new NUnit3Settings {
                ShadowCopy = false,
            });
    },
    new FilePath(pathCoverage),
    new OpenCoverSettings()
    .WithFilter("+[WcfService]*")
	.WithFilter("-[WcfService]WcfService.Properties.*")
    .WithFilter("-[WcfService.Tests]*"));
});

Task("Reporting")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    var reportOutput = tempDir + Directory("report-coverage");
    var unitTestOutput = tempDir + Directory("unit-test");

    ReportGenerator(pathCoverage, reportOutput);
    ReportUnit(Directory("./"), unitTestOutput, new ReportUnitSettings());

    var rerpotIndexFile = reportOutput + File("index.htm");
    var unitTestIndexFile = unitTestOutput + File("Index.html");

    Action<string> startPage = url => {
        if (FileExists(url))
        {
            System.Diagnostics.Process.Start(url);
        }
        else
        {
            Warning("Index Report not found");
        }
    };

    startPage(rerpotIndexFile);
    startPage(unitTestIndexFile);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Reporting");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);