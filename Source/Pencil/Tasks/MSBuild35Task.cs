using OpenFileSystem.IO;

namespace Pencil.Tasks
{
    public class MSBuild35Task : MSBuild3540Task
    {
        public MSBuild35Task()
        {
        }

        internal MSBuild35Task(IFileSystem fileSystem, IExecutionEnvironment platform) : base(fileSystem, platform)
        {
        }

        protected override string FrameworkDirectorySearchPattern
        {
            get { return "v3.5*"; }
        }
    }
}