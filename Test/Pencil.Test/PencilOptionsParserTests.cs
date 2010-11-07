using NUnit.Framework;
using OpenFileSystem.IO.FileSystem.Local;
using System.Linq;

namespace Pencil.Test
{
    [TestFixture]
    public class PencilOptionsParserTests
    {
        private PencilOptionsParser sut;

        [SetUp]
        public void Setup()
        {
            sut = new PencilOptionsParser();
        }

        [Test]
        public void Should_parse_assembly_reference()
        {
            var result = sut.Parse("-r:MyAssembly.dll");

            CollectionAssert.AreEquivalent(new[]{new Path("MyAssembly.dll")}, result.Assemblies);
        }

        [Test]
        public void Should_parse_assembly_references()
        {
            var result = sut.Parse("-r:MyAssembly.dll", "-r:MyOtherAssembly.dll");

            CollectionAssert.AreEquivalent(new[] { new Path("MyAssembly.dll"), new Path("MyOtherAssembly.dll") }, result.Assemblies.ToArray());
        }

        [Test]
        public void Should_parse_build_script()
        {
            var result = sut.Parse("-r:Whatever.dll", "BuildScript.cs", "-r:Whatever2.dll");

            Assert.AreEqual("BuildScript.cs", result.BuildScript);
        }

        [Test]
        public void Should_parse_targets()
        {
            var result = sut.Parse("-r:Whatever.dll", "BuildScript.cs", "-r:Whatever2.dll", "target1", "target2");

            Assert.AreEqual(new[]{"target1", "target2"}, result.Targets);
        }
    }
}