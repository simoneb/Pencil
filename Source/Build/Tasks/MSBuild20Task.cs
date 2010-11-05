using System.Linq;
using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;

namespace Pencil.Build.Tasks
{
    public class MSBuild20Task : MSBuildTask
    {
        public MSBuild20Task(IFileSystem fileSystem, IExecutionEnvironment platform) : base(fileSystem, platform)
        {
        }

        protected override Path GetMSBuildPath()
        {
            return FrameworksDirectory.Directories("v2.0*").First().GetFile("msbuild.exe").Path;
        }

        protected override void AppendAdditionalArguments(CommandLineBuilder builder)
        {
            
        }
    }
}