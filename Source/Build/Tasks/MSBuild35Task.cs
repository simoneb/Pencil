using System.Linq;
using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;

namespace Pencil.Build.Tasks
{
    public class MSBuild35Task : MSBuild3540Task
    {
        public MSBuild35Task(IFileSystem fileSystem, IExecutionEnvironment platform) : base(fileSystem, platform)
        {
        }

        protected override Path GetMSBuildPath()
        {
            return FrameworksDirectory.Directories("v3.5*").First().GetFile("msbuild.exe").Path;
        }
    }
}