using System.Linq;
using Pencil.IO;

namespace Pencil.Build.Tasks
{
    public class MSBuild35Task : MSBuild3540Task
    {
        public MSBuild35Task(IFileSystem fileSystem, IExecutionEnvironment platform) : base(fileSystem, platform)
        {
        }

        protected override Path GetMSBuildPath()
        {
            return FileSystem.GetDirectories(FrameworksDirectory, "v3.5").First() + "msbuild.exe";
        }
    }
}