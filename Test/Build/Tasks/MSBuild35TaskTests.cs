using Pencil.Build.Tasks;
using Pencil.Test.Stubs;

namespace Pencil.Test.Build.Tasks
{
    class MSBuild35TaskTests : MSBuild3540TaskTestsBase<MSBuild35Task>
    {
        protected override MSBuild35Task CreateTask(FileSystemStub fileSystem, ExecutionEnvironmentStub executionEnvironment)
        {
            return new MSBuild35Task(fileSystem, executionEnvironment);
        }

        protected override string ExpectedMSBuildPathFragment
        {
            get { return "v3.5"; }
        }
    }
}