using System.Linq;
using Pencil.IO;

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
            return FileSystem.GetDirectories(FrameworksDirectory, "v4.0*").First() + "msbuild.exe";
        }
    }
}