#tool "nuget:?package=NUnit.ConsoleRunner"
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=ReportGenerator"

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
var reportOutput = Directory(System.IO.Path.GetTempPath()) + Directory("report-coverage");

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
    .WithFilter("-[WcfService.Tests]*"));

});

Task("Reporting")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    ReportGenerator(pathCoverage, reportOutput);

    var indexFile = reportOutput + File("index.htm");

    if (FileExists(indexFile))
    {
        System.Diagnostics.Process.Start(indexFile);
    }
    else
    {
        Warning("Index Report not found");
    }
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