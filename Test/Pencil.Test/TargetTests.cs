using System.Collections.Generic;
using NUnit.Framework;
using Pencil.Test.Stubs;

namespace Pencil.Test
{
    [TestFixture]
	public class TargetTests
	{
		[Test]
		public void Should_run_dependencies_before_self()
		{
			var targetsExecuted = new List<string>();
			var dependencies = new List<string>();
			dependencies.Add("Core");
			dependencies.Add("Build");

			var project = new ProjectStub();
			project.RunHandler = targetsExecuted.Add;
			var target = new TargetStub(project, dependencies);
			target.ExecuteHandler = () => Assert.AreEqual(dependencies, targetsExecuted);
			target.Execute();
		}
	}
}