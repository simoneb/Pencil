using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;

namespace Pencil.Build.Tasks
{
	public class ExecTask : ExecTaskBase
	{
	    private readonly Path program;

	    public ExecTask(IFileSystem fileSystem, IExecutionEnvironment platform, Path program) : base(fileSystem, platform)
	    {
	        this.program = program;
	    }

	    protected override Path GetProgramCore()
	    {
	        return program;
	    }

	    protected override string GetArgumentsCore()
	    {
	        return Arguments;
	    }

	    public string Arguments { get; set; }
	}
}