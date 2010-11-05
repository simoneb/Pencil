using System.Linq;
using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;

namespace Pencil.Build.Tasks
{
    public class MSBuild40Task : MSBuild3540Task
    {
        public MSBuild40Task(IFileSystem fileSystem, IExecutionEnvironment platform)
            : base(fileSystem, platform)
        {
        }

        protected override Path GetMSBuildPath()
        {
            return FrameworksDirectory.Directories("v4.0*").First().GetFile("msbuild.exe").Path;
        }
    }
}