using System.Collections.Generic;
using NUnit.Framework;
using Pencil.Test.Stubs;

namespace Pencil.Test
{
    [TestFixture]
    public class ProgramTests
    {
        private static Program Program { get { return new Program(Logger.Null, x => null); } }

        [Test]
        public void BuildTarget_should_return_Failure_if_target_not_in_Project()
        {
            var project = new ProjectStub {HasTargetHandler = x => false};

            Assert.That(Program.BuildTarget(project, "Target"), Is.EqualTo(Program.Failure));
        }

        [Test]
        public void BuildTarget_should_return_Success_if_valid_target_and_successful_build()
        {
            var project = new ProjectStub {HasTargetHandler = x => true};

            Assert.That(Program.BuildTarget(project, "Target"), Is.EqualTo(Program.Success));
        }

        [Test]
        public void Should_build_all_specified_targets()
        {
            var project = new ProjectStub();
            var built = new List<string>();
            project.HasTargetHandler = x => true;
            project.RunHandler = built.Add;

			new Program(Logger.Null, x => project).Run(new StubOptions("BuildFile", "Target1", "Target2"));

			built.ShouldEqual(new[]{ "Target1", "Target2" });
        }

        [Test]
        public void Should_run_default_target_if_no_target_specified()
        {
            var project = new ProjectStub();
            var built = new List<string>();
            project.HasTargetHandler = x => true;
            project.RunHandler = built.Add;
            project.DefaultTargetHandler = x => "Target1";

            new Program(Logger.Null, x => project).Run(new StubOptions("BuildFile"));

            built.ShouldEqual(new[] { "Target1" });
        }

        [Test]
        public void Should_return_failure_if_no_default_and_no_targets_specified()
        {
            var project = new ProjectStub();
            var built = new List<string>();
            project.HasTargetHandler = x => true;
            project.RunHandler = built.Add;
            project.DefaultTargetHandler = x => null;

            new Program(Logger.Null, x => project).Run(new StubOptions("BuildFile"));

            built.ShouldBeEmpty();
        }

        [Test]
        public void Should_not_build_default_target_if_explicit_targets_are_supplied()
        {
            var project = new ProjectStub();
            var built = new List<string>();
            project.HasTargetHandler = x => true;
            project.RunHandler = built.Add;
            project.DefaultTargetHandler = x => "Default";

            new Program(Logger.Null, x => project).Run(new StubOptions("BuildFile", "Target1", "Target2" ));

            Assert.That(built, Contains.Item("Target1").And.Contains("Target2"));
            CollectionAssert.DoesNotContain(built, "Default");
        }

        [Test]
        public void Project_should_have_its_source_scripts_folder_as_current_directory()
        {
            var project = new ProjectStub
                          {
                              HasTargetHandler = x => true,
                              PlatformHandler = () => new ExecutionEnvironmentStub()
                          };

            string projectCurrentDirectory = null;
            project.RunHandler = s => projectCurrentDirectory = project.Platform.CurrentDirectory;
            
            new Program(Logger.Null, x => project).Run(new StubOptions(@"path\to\BuildFile", "whatever"));

            StringAssert.EndsWith(@"path\to", projectCurrentDirectory);
        }

        [Test]
        public void Original_current_directory_should_be_restored_when_finished()
        {
            var project = new ProjectStub
                          {
                              HasTargetHandler = x => true,
                              PlatformHandler = () => new ExecutionEnvironmentStub()
                          };

            var originalDirectory = project.Platform.CurrentDirectory;

            new Program(Logger.Null, x => project).Run(new StubOptions(@"path\to\BuildFile", "whatever" ));

            Assert.AreEqual(originalDirectory, project.Platform.CurrentDirectory);
        }

        [Test]
        public void Should_display_options_if_no_build_script_supplier()
        {
            var spyOptions = new SpyOptions { BuildScript = null };
            Program.Run(spyOptions);
            Assert.IsTrue(spyOptions.Displayed);
        }
    }
}