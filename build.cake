#tool "nuget:?package=NUnit.ConsoleRunner"
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=ReportGenerator"
#tool "nuget:?package=Machine.Specifications.runner.console"
#tool "nuget:?package=ReportUnit"
#addin Cake.VsMetrics
// #addin "Cake.CodeAnalysisReporting"

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
    
    // CreateMsBuildCodeAnalysisReport(
    //     "./msbuild.log",
    //     CodeAnalysisReport.MsBuildXmlFileLoggerByAssembly,
    //     "./msbuild_output.html");
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
        settings.SetConfiguration(configuration)
        .AddFileLogger(new MSBuildFileLogger
        {
            Encoding = "UTF-8"
        }));
});

Task("vs-metrics")
    .IsDependentOn("Build")
    .Does(() => {
        var projects = GetFiles("./WcfService/bin/WcfService.dll");
        var settings = new VsMetricsSettings()
        {
            SuccessFile = true,
            IgnoreGeneratedCode = true
        };

        VsMetrics(projects, "./metrics_result.xml", settings);
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

Task("Run-Spec-Tests")
    .IsDependentOn("Build")
    .Does(() => {
        var testAssemblies = GetFiles("./WcfService.Specs/bin/" + configuration + "/*.Specs.dll");

        MSpec(testAssemblies);
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