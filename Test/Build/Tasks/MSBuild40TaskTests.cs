using Pencil.Build.Tasks;
using Pencil.Test.Stubs;

namespace Pencil.Test.Build.Tasks
{
    class MSBuild40TaskTests : MSBuild3540TaskTestsBase<MSBuild40Task>
    {
        protected override MSBuild40Task CreateTask(FileSystemStub fileSystem, ExecutionEnvironmentStub executionEnvironment)
        {
            return new MSBuild40Task(fileSystem, executionEnvironment);
        }

        protected override string ExpectedMSBuildPathFragment
        {
            get { return "v4.0"; }
        }
    }
}