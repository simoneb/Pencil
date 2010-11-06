using OpenFileSystem.IO;
using Pencil.Build;
using Pencil.Build.Tasks;
using Path = OpenFileSystem.IO.FileSystem.Local.Path;

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
        var dist = FileSystem.GetDirectory("dist");

        if (dist.Exists)
            dist.Delete();

        dist.MustExist();

        foreach (var file in FileSystem.GetDirectory(@"Source\bin\Release").Files())
            file.CopyTo(dist.GetFile(file.Name));
    }

    MSBuild40Task NewMSBuildTask()
    {
        return new MSBuild40Task(FileSystem, Platform);
    }
}
