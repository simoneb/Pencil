using OpenFileSystem.IO;

namespace Pencil.Tasks
{
    public class MSBuild20Task : MSBuildTask
    {
        public MSBuild20Task(IFileSystem fileSystem, IExecutionEnvironment platform) : base(fileSystem, platform)
        {
        }

        protected override string FrameworkDirectorySearchPattern
        {
            get { return "v2.0*"; }
        }
    }
}