using NUnit.Framework;
using Pencil.Attributes;
using Pencil.Test.Stubs;
using System.Linq;

namespace Pencil.Test
{
	[TestFixture]
	public class MethodTargetTests
	{
		class MyProject : ProjectStub
		{
			[DependsOn("Build"), DependsOn("Test")]
			public void All(){}

            [Default]
            [Attributes.Description("Builds the project")]
			public void Build(){}
			public void Test(){}
		}

		[Test]
		public void Should_get_dependencies_from_DependsOnAttribute()
		{
		    var target = GetTarget("All");

		    Assert.That(new[]{ "Build", "Test"}, Is.EquivalentTo(target.GetDependencies().ToArray()));
		}

	    private static MethodTarget GetTarget(string name)
	    {
	        var project = new MyProject();
	        return new MethodTarget(project, project.GetType().GetMethod(name));
	    }

	    [Test]
        public void Should_tell_if_default()
        {
            var target = GetTarget("Build");

            Assert.IsTrue(target.IsDefault);
        }

        [Test]
        public void Should_tell_if_not_default()
        {
            var target = GetTarget("All");

            Assert.IsFalse(target.IsDefault);
        }

        [Test]
        public void Should_get_description()
        {
            var target = GetTarget("Build");
            Assert.AreEqual("Builds the project", target.Description);
        }

        [Test]
        public void Should_get_description_as_empty_string_when_not_specified()
        {
            var target = GetTarget("All");
            Assert.AreEqual("", target.Description);
        }
	}
}