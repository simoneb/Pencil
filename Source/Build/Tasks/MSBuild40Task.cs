using OpenFileSystem.IO;

namespace Pencil.Build.Tasks
{
    public class MSBuild40Task : MSBuild3540Task
    {
        public MSBuild40Task(IFileSystem fileSystem, IExecutionEnvironment platform) : base(fileSystem, platform)
        {
        }

        protected override string FrameworkDirectorySearchPattern
        {
            get { return "v4.0*"; }
        }
    }
}