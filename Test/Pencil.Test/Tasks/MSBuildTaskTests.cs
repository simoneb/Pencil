using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.InMemory;
using Pencil.Tasks;
using Pencil.Test.Stubs;

namespace Pencil.Test.Tasks
{
    [TestFixture]
    public abstract class MSBuildTaskTests<TTask> where TTask : MSBuildTask
    {
        private ExecutionEnvironmentStub executionEnvironment;
        private InMemoryFileSystem fileSystem;
        private TTask task;

        [SetUp]
        public void Setup()
        {
            executionEnvironment = new ExecutionEnvironmentStub();
            fileSystem = new InMemoryFileSystem();
            task = CreateTask(fileSystem, executionEnvironment);

            fileSystem.GetDirectory(RuntimeEnvironment.GetRuntimeDirectory()).Parent.Create().GetOrCreateDirectory(ExpectedMSBuildPathFragment + "1345")
    .GetFile("msbuild.exe").Create();
        }

        protected abstract TTask CreateTask(IFileSystem fileSystem, IExecutionEnvironment executionEnvironment);

        [TestCase(new[] { "log.txt", "solution.sln" }, "solution.sln")]
        [TestCase(new[] { "solution.sln", "log.txt" }, "solution.sln")]
        [TestCase(new[] { "solution1.sln", "solution2.sln" }, "solution1.sln")]
        [TestCase(new[] { "solution2.sln", "solution1.sln" }, "solution2.sln")]
        public void Should_pick_first_sln_if_no_project_supplied(string[] files, string expectedChoice)
        {

            foreach (var file in files)
                fileSystem.GetFile(file).Create();

            executionEnvironment.RunHandler = (fileName, arguments, processHandler) =>
                                              {
                                                  Assert.That(arguments, Contains.Substring(expectedChoice));
                                              };

            task.Run();
        }

        [Test]
        public void Should_accept_project_file()
        {
            CheckArgument(t => t.ProjectFile = "mySolution.sln", " mySolution.sln");
        }

        [Test]
        public void Should_accept_targets()
        {
            CheckArgument(t => t.Targets = new[]{"compile", "test"}, " /target:compile;test");
        }

        [Test]
        public void Should_accept_single_property()
        {
            CheckArgument(t => t.AddProperty("prop1", "value1"), " /property:prop1=value1");
        }

        [Test]
        public void Should_accept_multiple_properties()
        {
            CheckArgument(t =>
                          {
                              t.AddProperty("prop1", "value1");
                              t.AddProperty("prop2", "value2");
                          }, " /property:prop1=value1 /property:prop2=value2");
        }

        [Test]
        public void Should_quote_switch_with_spaces()
        {
            CheckArgument(t => t.AddProperty("prop1", "some value"), " /property:\"prop1=some value\"");            
        }

        [TestCase(MSBuildVerbosity.Quiet, "quiet")]
        [TestCase(MSBuildVerbosity.Minimal, "minimal")]
        [TestCase(MSBuildVerbosity.Normal, "normal")]
        [TestCase(MSBuildVerbosity.Detailed, "detailed")]
        [TestCase(MSBuildVerbosity.Diagnostic, "diagnostic")]
        public void Should_accept_verbosity(MSBuildVerbosity verbosity, string verbosityString)
        {
            CheckArgument(t => t.Verbosity = verbosity, " /verbosity:" + verbosityString);
        }

        [Test]
        public void Should_not_set_verbosity_if_not_supplied()
        {
            CheckArgumentMissing("/verbosity:");
        }

        protected void CheckArgument(Action<TTask> setProperty, string stringToLookFor)
        {
            setProperty(task);

            executionEnvironment.RunHandler = (fileName, arguments, processHandler) =>
                                              {
                                                  Assert.That(arguments, Contains.Substring(stringToLookFor));
                                              };

            task.Run();
        }

        protected void CheckArgumentMissing(string argument)
        {
            executionEnvironment.RunHandler = (fileName, arguments, processHandler) =>
            {
                arguments.Contains(argument).ShouldBe(false);
            };

            task.Run();
        }

        [Test]
        public void Should_locate_MSBuild()
        {
            executionEnvironment.RunHandler = (fileName, arguments, processHandler) =>
            {
                Assert.That(fileName, Contains.Substring(ExpectedMSBuildPathFragment));
            };

            task.Run();
        }

        protected abstract string ExpectedMSBuildPathFragment { get; }
    }
}