import System;
import System.Net;
import OpenFileSystem.IO;
import OpenFileSystem.IO.FileSystem.Local;
import Pencil;
import Pencil.Attributes;
import Pencil.Tasks;

class PencilProject extends Project
{
	DependsOn("Clean") 
    Description("Builds the project and produces the output binaries")
    function  Build()
	{
	    var msbuild = NewMSBuildTask();
        msbuild.ShowCommandLine = true;
	    msbuild.ProjectFile = "Pencil.sln";
        msbuild.AddProperty("Configuration", "Release");
        msbuild.AddProperty("Platform", "Any CPU");
        //msbuild.Verbosity = MSBuildVerbosity.Quiet;
	    msbuild.Targets = ["Rebuild"];

        msbuild.Execute();
    }

    Description("Cleans the artifacts generated during the build process")
	function Clean()
	{
        var msbuild = NewMSBuildTask();
        msbuild.ShowCommandLine = true;
        msbuild.ProjectFile = "Pencil.sln";
        msbuild.AddProperty("Configuration", "Release");
        msbuild.AddProperty("Platform", "Any CPU");
        //msbuild.Verbosity = MSBuildVerbosity.Quiet;
        msbuild.Targets = ["Clean"];

        msbuild.Execute();

        FileSystem.GetDirectory("dist").Delete();
        FileSystem.GetDirectory("merged").Delete();
	}

    Default()
    DependsOn("Build")
    Description("Runs the tests")
    function Test()
    {
        with(new NUnitTask(FileSystem, Platform))
        {
            NUnitBinPath = new Path("Tools\\NUnit");
            Target = new Path("Test\\Pencil.Test\\bin\\Release\\Pencil.Test.dll");
            Execute();
        }
    }

    DependsOn("Build")
    DependsOn("Test")
    Description("Copies the output to the distribution folder")
    function Dist()
    {
        var dist = FileSystem.GetDirectory("dist");

        if (dist.Exists)
            dist.Delete();
        
        dist = FileSystem.CreateDirectory("dist");

        for (var file in FileSystem.GetDirectory("Source\\Pencil\\bin\\Release").Files().GetEnumerator())
            file.CopyTo(dist.GetFile(file.Name));
    }

    DependsOn("Dist")
    Description("Merges the output using ILMerge")
    function Merge()
    {
        var ilmergeExe = "Tools\\ilmerge\\SourceDir\\ILMerge.exe";

        if (!FileSystem.GetFile(ilmergeExe).Exists)
            ExtractILMerge(DownloadILMerge(FileSystem.GetTempDirectory()), "Tools\\ilmerge");

        RunILMerge(ilmergeExe, "merged");
    }

    function RunILMerge(ilmergeExe, output)
    {
        var outputDir = FileSystem.GetDirectory(output);
        
        if (outputDir.Exists)
            outputDir.Delete();
        
        outputDir = FileSystem.CreateDirectory(output);

        var commandLine = String.Format("/t:exe /xmldocs /out:{0}\\Pencil.exe {1}\\Pencil.exe {1}\\OpenFileSystem.dll", output, "dist");

        with(new ExecTask(FileSystem, Platform, new Path(ilmergeExe)))
        {
            Arguments = commandLine;
            ShowCommandLine = true;
            Execute();
        }
    }

    function ExtractILMerge(msiPath, destinationDirectory)
    {
        var destDir = FileSystem.GetDirectory(destinationDirectory);

        if (destDir.Exists) {
            destDir.Delete();
        }
        
        destDir = FileSystem.CreateDirectory(destinationDirectory);

        with(new ExecTask(FileSystem, Platform, new Path("Tools\\lessmsi\\lessmsi.exe")))
        {
            Arguments = "/x " + msiPath + " " + destinationDirectory;
            ShowCommandLine = true;
            Execute();
        }
    }

    function DownloadILMerge(destination)
    {
        var destinationFile = destination.GetFile(Guid.NewGuid() + ".msi").Path.FullPath;

        var c = new WebClient();
            c.DownloadFile("http://download.microsoft.com/download/1/3/4/1347C99E-9DFB-4252-8F6D-A3129A069F79/ILMerge.msi", destinationFile);

        return destinationFile;
    }

    function NewMSBuildTask() : MSBuild3540Task
    {
        return new MSBuild40Task(FileSystem, Platform);
    }
}
