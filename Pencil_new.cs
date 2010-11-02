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
	    msbuild.ProjectFile = "Pencil.sln";
        msbuild.AddProperty("OutDir", "..\\Dist\\");
        msbuild.Verbosity = MSBuildVerbosity.Normal;
	    msbuild.Targets = new[] {"Rebuild"};

        msbuild.Execute();
    }

	public void Clean()
	{
        var msbuild = NewMSBuildTask();
        msbuild.ProjectFile = "Pencil.sln";
        msbuild.AddProperty("OutDir", "..\\Dist\\");
        msbuild.Targets = new[] { "Clean" };

        msbuild.Execute();
	}

    MSBuild40Task NewMSBuildTask()
    {
        return new MSBuild40Task(FileSystem, Platform);
    }
}
