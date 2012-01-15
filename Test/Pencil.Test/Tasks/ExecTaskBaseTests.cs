using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.InMemory;
using OpenFileSystem.IO.FileSystem.Local;
using Pencil.Tasks;
using Pencil.Test.Stubs;
using NUnit.Framework;

namespace Pencil.Test.Tasks
{
    [TestFixture]
    public class ExecTaskBaseTests
    {
        class TestTask : ExecTaskBase
        {
            public TestTask(IFileSystem fileSystem, IExecutionEnvironment platform): base(fileSystem, platform)
            {}

            public override Path Program
            {
                get { return new Path("TestTask"); }
            }

            protected override  string GetArguments(){ return string.Empty; }
        }
    
        [Test]
        public void Execute_should_call_wait_for_exit_before_ExitCode()
        {//Since mono is broken and won't give us the ExitCode otherwise.
            var process = new ProcessStub();
            var waitForExitCalled = false;
            process.WaitForExitHandler = () => waitForExitCalled = true;
            process.GetExitCodeHandler = () =>
            { 
                Assert.IsTrue(waitForExitCalled);
                return 0;
            };
            var platform = new ExecutionEnvironmentStub();
            var fileSystem = new InMemoryFileSystem();
            platform.RunHandler = (program, args, handler) => handler(process);
            var task = new TestTask(fileSystem, platform);
            task.Run();
        }
    }
}
