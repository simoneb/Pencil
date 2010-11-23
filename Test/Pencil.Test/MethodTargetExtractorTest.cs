using System.Collections.Generic;
using NUnit.Framework;

namespace Pencil.Test
{
    [TestFixture]
    public class MethodTargetExtractorTest
    {
        private Dictionary<string, Target>.KeyCollection targets;

        class MyProj : BaseProject
        {
            public void Target(){}

            public string MyProp { get; set; }
        }

        internal class BaseProject : Project
        {
            public void BaseTarget() {}
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            targets = MethodTargetExtractor.GetTargets(new MyProj()).Keys;
        }

        [Test]
        public void Should_not_get_project_methods()
        {
            Assume.That(typeof(Project).GetMethod("HasTarget"), Is.Not.Null);

            Assert.That(targets, Has.No.Member(("HasTarget")));
        }

        [Test]
        public void Should_not_get_Object_methods()
        {
            Assert.That(targets, Has.No.Member(("ToString")));
        }

        [Test]
        public void Should_not_get_properties()
        {
            Assert.That(targets, Has.No.Member("get_MyProp"));
            Assert.That(targets, Has.No.Member("set_MyProp"));
            Assert.That(targets, Has.No.Member("MyProp"));
        }

        [Test]
        public void Should_get_base_class_methods()
        {
            Assert.That(targets, Has.Member("BaseTarget"));
        }

        [Test]
        public void Should_get_own_methods()
        {
            Assert.That(targets, Has.Member("Target"));
        }
    }
}