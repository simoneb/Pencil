using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;

namespace Pencil.Tasks
{
    public abstract class CompilerBaseTask : ExecTaskBase
    {
        readonly FileSet sources;
		readonly FileSet references;

        public Path Output { get; set; }
        public FileSet Sources { get { return sources; } }
		public FileSet References { get { return references; } }

        protected CompilerBaseTask()
        {
            sources = new FileSet(FileSystem);
            references = new FileSet(FileSystem);
        }

        internal CompilerBaseTask(IFileSystem fileSystem, IExecutionEnvironment platform) : base(fileSystem, platform)
        {
            sources = new FileSet(FileSystem);
		    references = new FileSet(FileSystem);
        }

        public void Compile()
        {
            if(Sources.ChangedAfter(Output) || References.ChangedAfter(Output))
                CompileCore();
        }

        protected virtual void CompileCore()
        {
            Run();
        }
    }
}