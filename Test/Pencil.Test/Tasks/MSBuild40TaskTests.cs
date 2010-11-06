using OpenFileSystem.IO;
using Pencil.Tasks;

namespace Pencil.Test.Tasks
{
    class MSBuild40TaskTests : MSBuild3540TaskTestsBase<MSBuild40Task>
    {
        protected override MSBuild40Task CreateTask(IFileSystem fileSystem, IExecutionEnvironment executionEnvironment)
        {
            return new MSBuild40Task(fileSystem, executionEnvironment);
        }

        protected override string ExpectedMSBuildPathFragment
        {
            get { return "v4.0"; }
        }
    }
}