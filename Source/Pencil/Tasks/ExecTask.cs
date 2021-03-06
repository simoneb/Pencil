using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;

namespace Pencil.Tasks
{
	public class ExecTask : ExecTaskBase
	{
	    private readonly Path program;

	    public ExecTask(Path program)
	    {
	        this.program = program;
	    }

	    internal ExecTask(IFileSystem fileSystem, IExecutionEnvironment platform, Path program) : base(fileSystem, platform)
	    {
	        this.program = program;
	    }

	    public override Path Program
	    {
	        get { return program; }
	    }

	    protected override string GetArguments()
	    {
	        return Arguments;
	    }

	    public string Arguments { get; set; }
	}
}