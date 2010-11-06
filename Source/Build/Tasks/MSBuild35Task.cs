using OpenFileSystem.IO;

namespace Pencil.Build.Tasks
{
    public class MSBuild35Task : MSBuild3540Task
    {
        public MSBuild35Task(IFileSystem fileSystem, IExecutionEnvironment platform) : base(fileSystem, platform)
        {
        }

        protected override string FrameworkDirectorySearchPattern
        {
            get { return "v3.5*"; }
        }
    }
}