using OpenFileSystem.IO;
using Pencil.Build;
using Pencil.Build.Tasks;
using Pencil.Test.Stubs;

namespace Pencil.Test.Build.Tasks
{
    public class MSBuild20TaskTests : MSBuildTaskTests<MSBuild20Task>
    {
        protected override MSBuild20Task CreateTask(IFileSystem fileSystem, IExecutionEnvironment executionEnvironment)
        {
            return new MSBuild20Task(fileSystem, executionEnvironment);
        }

        protected override string ExpectedMSBuildPathFragment
        {
            get { return "v2.0"; }
        }
    }
}