using System.Collections.Generic;
using NUnit.Framework;
using Pencil.Test.Stubs;

namespace Pencil.Test
{
    static class IEnumerableExtensions
	{
		public static List<T> All<T>(this IEnumerable<T> sequence)
		{
			return new List<T>(sequence);
		}
	}

	[TestFixture]
	public class MethodTargetTests
	{
		class MyProject : ProjectStub
		{
			[DependsOn("Build"), DependsOn("Test")]
			public void All(){}

			public void Build(){}
			public void Test(){}
		}

		[Test]
		public void Should_get_dependencies_from_DependsOnAttribute()
		{
			var project = new MyProject();
			var target = new MethodTarget(project, project.GetType().GetMethod("All"));

			Assert.That(new[]{ "Build", "Test"}, Is.EquivalentTo(target.GetDependencies().All()));
		}
	}
}