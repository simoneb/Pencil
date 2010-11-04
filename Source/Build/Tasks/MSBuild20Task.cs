using System.Linq;
using Pencil.IO;

namespace Pencil.Build.Tasks
{
    public class MSBuild20Task : MSBuildTask
    {
        public MSBuild20Task(IFileSystem fileSystem, IExecutionEnvironment platform) : base(fileSystem, platform)
        {
        }

        protected override Path GetMSBuildPath()
        {
            return FileSystem.GetDirectories(FrameworksDirectory, "v2.0*").First() + "msbuild.exe";
        }

        protected override void AppendAdditionalArguments(CommandLineBuilder builder)
        {
            
        }
    }
}