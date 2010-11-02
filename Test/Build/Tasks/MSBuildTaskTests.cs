using System;
using NUnit.Framework;
using Pencil.Build.Tasks;
using Pencil.IO;
using Pencil.Test.Stubs;
using System.Linq;

namespace Pencil.Test.Build.Tasks
{
    [TestFixture]
    public abstract class MSBuildTaskTests<TTask> where TTask : MSBuildTask
    {
        private ExecutionEnvironmentStub executionEnvironment;
        private FileSystemStub fileSystem;
        private TTask task;

        [SetUp]
        public void Setup()
        {
            executionEnvironment = new ExecutionEnvironmentStub();
            fileSystem = new FileSystemStub();
            task = CreateTask(fileSystem, executionEnvironment);

            fileSystem.GetDirectoriesHandler = (root, pattern) =>
                                               new[]
                                               {
                                                   new Path("whatever") + ExpectedMSBuildPathFragment + "12345",
                                               };
        }

        protected abstract TTask CreateTask(FileSystemStub fileSystem, ExecutionEnvironmentStub executionEnvironment);

        [TestCase(new[] { "log.txt, solution.sln" }, "solution.sln")]
        [TestCase(new[] { "solution.sln, log.txt" }, "solution.sln")]
        [TestCase(new[] { "solution1.sln, solution2.sln" }, "solution1.sln")]
        [TestCase(new[] { "solution2.sln, solution1.sln" }, "solution2.sln")]
        public void Should_pick_first_sln_if_no_project_supplied(string[] files, string expectedChoice)
        {
            fileSystem.GetFilesRecursiveHandler = (path, pattern) =>
                                                  {
                                                      return files.Select(f => new Path(f));
                                                  };

            executionEnvironment.RunHandler = (fileName, arguments, processHandler) =>
                                              {
                                                  arguments.Contains(expectedChoice).ShouldBe(true);
                                              };

            task.Execute();
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
                          }, " /property:prop1=value1;prop2=value2");
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

            task.Execute();
        }

        protected void CheckArgumentMissing(string argument)
        {
            executionEnvironment.RunHandler = (fileName, arguments, processHandler) =>
            {
                arguments.Contains(argument).ShouldBe(false);
            };

            task.Execute();
        }

        [Test]
        public void Should_locate_MSBuild()
        {
            executionEnvironment.RunHandler = (fileName, arguments, processHandler) =>
            {
                Assert.That(fileName, Contains.Substring(ExpectedMSBuildPathFragment));
            };

            task.Execute();
        }

        protected abstract string ExpectedMSBuildPathFragment { get; }
    }
}