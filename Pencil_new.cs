using Pencil.IO;
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
        msbuild.AddProperty("OutDir", "..\\Dist\\");
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
        msbuild.AddProperty("OutDir", "..\\Dist\\");
        msbuild.AddProperty("Configuration", "Release");
        msbuild.AddProperty("Platform", "Any CPU");
        msbuild.Targets = new[] { "Clean" };

        msbuild.Execute();
	}

    [Default]
    [DependsOn("Build")]
    public void Test()
    {
        new NUnitTask(Platform)
                    {
                        NUnitBinPath = new Path("Tools") + "NUnit",
                        Target = new Path("Dist") + "Pencil.Test.dll"
                    }.Execute();
    }

    MSBuild40Task NewMSBuildTask()
    {
        return new MSBuild40Task(FileSystem, Platform);
    }
}
