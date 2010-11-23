using System;
using System.IO;
using NUnit.Framework;

namespace Pencil.Test
{
    [TestFixture]
    public class IncludeTests
    {
        private MyProject project;

        class MyProject : Project
        {
            public MyProject(string includePath)
            {
                Include(includePath);
            }

            public void MyTarget()
            {
                Logger.WriteLine("My output");
            }

            public int IncludeProperty
            {
                get { return Property<int>("IncludeProperty"); }
            }
        }

        [SetUp]
        public void Setup()
        {
            const string includescript = "IncludeScript.cs";

            Assume.That(File.Exists(includescript), string.Format("File {0} should exist", includescript));

            project = new MyProject(includescript);
        }

        [Test]
        public void Should_throw_when_script_does_not_exist()
        {
            Assert.Throws<InvalidOperationException>(() => new MyProject("NonExistentFile.cs"));
        }

        [Test]
        public void Should_get_include_property()
        {
            Assert.AreEqual(2, project.IncludeProperty);
        }

        [Test]
        public void Should_call_target_in_included_script()
        {
            var spyLogger = new SpyLogger();
            project.Logger = spyLogger;
            project.Run("IncludeTarget");

            Assert.That(spyLogger.Written, Contains.Substring("Include output"));
        }

        [Test]
        public void Should_call_target_in_included_script_and_then_own_target()
        {
            var spyLogger = new SpyLogger();
            project.Logger = spyLogger;
            project.Run("IncludeTarget");
            project.Run("MyTarget");

            Assert.That(spyLogger.Written, Contains.Substring("Include output"));
            Assert.That(spyLogger.Written, Contains.Substring("My output"));
        }
    }

    public class SpyLogger : Logger
    {
        public SpyLogger() : base(new StringWriter())
        {
        }

        public string Written { get { return ((StringWriter) Target).GetStringBuilder().ToString(); } }
    }
}