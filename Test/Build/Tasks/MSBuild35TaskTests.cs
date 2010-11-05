using OpenFileSystem.IO;
using Pencil.Build;
using Pencil.Build.Tasks;

namespace Pencil.Test.Build.Tasks
{
    class MSBuild35TaskTests : MSBuild3540TaskTestsBase<MSBuild35Task>
    {
        protected override MSBuild35Task CreateTask(IFileSystem fileSystem, IExecutionEnvironment executionEnvironment)
        {
            return new MSBuild35Task(fileSystem, executionEnvironment);
        }

        protected override string ExpectedMSBuildPathFragment
        {
            get { return "v3.5"; }
        }
    }
}