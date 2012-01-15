using System;
using System.Net;
using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;
using Pencil;
using Pencil.Attributes;
using Pencil.Tasks;

public class PencilProject : Project
{
    [DependsOn("Clean")]
    [Description("Builds the project and produces the output binaries")]
	public void Build()
    {
        new MSBuild40Task(FileSystem, Platform) {
            ShowCommandLine = true,
            ProjectFile = "Pencil.sln",
            Properties = {{"Configuration", "Release"}, {"Platform", "Any CPU"}},
            Verbosity = MSBuildVerbosity.Quiet,
            Targets = {"Rebuild"} }.Run();
    }

    [Description("Cleans the artifacts generated during the build process")]
	public void Clean()
	{
        new MSBuild40Task(FileSystem, Platform) {
            ShowCommandLine = true,
            ProjectFile = "Pencil.sln",
            Properties = {{"Configuration", "Release"}, {"Platform", "Any CPU"}},
            Verbosity = MSBuildVerbosity.Quiet,
            Targets = {"Clean"} }.Run();

        FileSystem.GetDirectory("dist").Delete();
        FileSystem.GetDirectory("merged").Delete();
	}

    [Default]
    [DependsOn("Build")]
    [Description("Runs the tests")]
    public void Test()
    {
        new NUnitTask(FileSystem, Platform) {
            NUnitBinPath = new Path(@"Tools\NUnit"),
            Target = new Path(@"Test\Pencil.Test\bin\Release\Pencil.Test.dll") }.Run();
    }

    [DependsOn("Build")]
    [DependsOn("Test")]
    [Description("Copies the output to the distribution folder")]
    public void Dist()
    {
        var dist = FileSystem.GetDirectory("dist");

        if (dist.Exists)
            dist.Delete();

        dist.MustExist();

        foreach (var file in FileSystem.GetDirectory(@"Source\Pencil\bin\Release").Files())
            file.CopyTo(dist.GetFile(file.Name));
    }

    [DependsOn("Dist")]
    [Description("Merges the output using ILMerge")]
    public void Merge()
    {
        const string ilmergeExe = @"Tools\ilmerge\SourceDir\ILMerge.exe";

        if (!FileSystem.GetFile(ilmergeExe).Exists)
            ExtractILMerge(DownloadILMerge(FileSystem.GetTempDirectory()), @"Tools\ilmerge");

        RunILMerge(ilmergeExe, "merged");
    }

    private void RunILMerge(string ilmergeExe, string output)
    {
        FileSystem.GetDirectory(output).MustExist();

        var commandLine = string.Format(@"/t:exe /xmldocs /out:{0}\Pencil.exe {1}\Pencil.exe {1}\OpenFileSystem.dll",
                                      output, "dist");

        new ExecTask(FileSystem, Platform, new Path(ilmergeExe))
        {
            Arguments = commandLine,
            ShowCommandLine = true
        }.Run();
    }

    private void ExtractILMerge(string msiPath, string destinationDirectory)
    {
        FileSystem.GetDirectory(destinationDirectory).MustExist();

        new ExecTask(FileSystem, Platform, new Path(@"Tools\lessmsi\lessmsi.exe"))
        {
            Arguments = "/x " + msiPath + " " + destinationDirectory,
            ShowCommandLine = true
        }.Run();
    }

    private string DownloadILMerge(IDirectory destination)
    {
        var destinationFile = destination.GetFile(Guid.NewGuid() + ".msi").Path.FullPath;

        using (var c = new WebClient())
            c.DownloadFile("http://download.microsoft.com/download/1/3/4/1347C99E-9DFB-4252-8F6D-A3129A069F79/ILMerge.msi", destinationFile);

        return destinationFile;
    }
}
