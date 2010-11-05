using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;
using Pencil.Build;
using Pencil.Build.Tasks;

public class PencilProject : Project
{
	IFileSystem FileSystem { get { return New<IFileSystem>(); } }
	IExecutionEnvironment Platform { get { return New<IExecutionEnvironment>(); } }

    [DependsOn("Clean")]
	public void Build()
	{
	    var msbuild = NewMSBuildTask();
        msbuild.ShowCommandLine = true;
	    msbuild.ProjectFile = "Pencil.sln";
        msbuild.AddProperty("Configuration", "Release");
        msbuild.AddProperty("Platform", "Any CPU");
        msbuild.Verbosity = MSBuildVerbosity.Normal;
	    msbuild.Targets = new[] {"Rebuild"};

        msbuild.Execute();
    }

	public void Clean()
	{
        var msbuild = NewMSBuildTask();
        msbuild.ShowCommandLine = true;
        msbuild.ProjectFile = "Pencil.sln";
        msbuild.AddProperty("Configuration", "Release");
        msbuild.AddProperty("Platform", "Any CPU");
        msbuild.Targets = new[] { "Clean" };

        msbuild.Execute();
	}

    [Default]
    [DependsOn("Build")]
    public void Test()
    {
        new NUnitTask(FileSystem, Platform)
                    {
                        NUnitBinPath = new Path(@"Tools\NUnit"),
                        Target = new Path(@"Test\bin\Release\Pencil.Test.dll")
                    }.Execute();
    }

    [DependsOn("Build")]
    [DependsOn("Test")]
    public void Dist()
    {
        var dist = "dist";

        //if(Directory.Exists(dist))
        //    Directory.Delete(dist, true);

        //Directory.CreateDirectory(dist);

        //foreach (var file in Directory.GetFiles("Source/bin/Release"))
        //    File.Copy(file, System.IO.Path.Combine(dist, System.IO.Path.GetFileName(file)));
    }

    MSBuild40Task NewMSBuildTask()
    {
        return new MSBuild40Task(FileSystem, Platform);
    }
}
