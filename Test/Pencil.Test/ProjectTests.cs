using Pencil.Tasks;
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Pencil.Test
{
    [TestFixture]
	public class ProjectTests
	{
		[Test]
		public void Should_be_able_to_create_CSharpCompilerTask()
		{
			var project = new Project();

			Assert.IsNotNull(project.New<CSharpCompilerTask>());
		}

		public class DoubleBuildBug : SpyProject
		{
		    public void Core(){}

		    [DependsOn("Core")]
			public void Build(){}

		    [DependsOn("Core"), DependsOn("Build")]
		    public void Test(){}
		}

        class WithDefaultTarget : Project
        {
            public void Clean() {}

            [Default]
            public void Build() {}

            public void Release() {}
        }

        class NoAmbiguities : SpyProject
        {
            public void Build(){}
            public void Test(){}
        }

        class CaseAmbiguities : SpyProject
        {
            public void build(){}
            public void Build(){}
        }

		[Test]
		public void Wont_runt_same_target_multiple_times()
		{
			var targetsBuilt = new List<string>();
			var project = new DoubleBuildBug();
			project.RunHandler += target =>  targetsBuilt.Add(target.Name);
			project.Run("Test");
			Assert.That(targetsBuilt, Is.EquivalentTo(new[]{ "Test", "Core", "Build" }));
		}

        [Test]
        public void Target_names_are_case_insensitive()
        {
            var targetsBuilt = new List<string>();
            var project = new NoAmbiguities();
            project.RunHandler += target => targetsBuilt.Add(target.Name);
            project.Run("build");
            Assert.That(targetsBuilt, Is.EquivalentTo(new[] { "Build" }));
        }

        [Test]
        public void Should_not_build_target_twice_due_to_case_sensitivities_issues()
        {
            var targetsBuilt = new List<string>();
            var project = new DoubleBuildBug();
            project.RunHandler += target => targetsBuilt.Add(target.Name);
            project.Run("test");
            Assert.That(targetsBuilt, Is.EquivalentTo(new[] { "Test", "Core", "Build" }));
        }

        [Test]
        public void Should_throw_if_targets_differ_by_case()
        {
           Assert.Throws<DuplicateTargetException>(() => new CaseAmbiguities(), "Target names are case insensitive, duplicates not allowed ('Build', 'build')");
        }

        [Test]
        public void Default_target_should_be_null_if_no_default_target_specified()
        {
            Assert.IsNull(new DoubleBuildBug().DefaultTarget);
        }

        [Test]
        public void Default_target_should_be_the_name_of_default_target()
        {
            Assert.AreEqual("Build", new WithDefaultTarget().DefaultTarget);
        }
	}

    public class SpyProject : Project
    {
        public Action<Target> RunHandler = ignored => {};

        protected override void RunCore(Target target)
        {
            RunHandler(target);
            base.RunCore(target);
        }
    }
}