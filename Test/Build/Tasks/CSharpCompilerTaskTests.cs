using Pencil.Core;
using Assembly = System.Reflection.Assembly;

namespace Pencil.Test.Build.Tasks
{
    using NUnit.Framework;
    using Pencil.Build.Tasks;
    using Stubs;
    using IO;

    [TestFixture]
    public class CSharpCompilerTaskTests
    {
        private CSharpCompilerTask compiler;
        private ExecutionEnvironmentStub environment;
        private FileSystemStub fileSystem;

        [SetUp]
        public void Setup()
        {
            fileSystem = new FileSystemStub();
            environment = new ExecutionEnvironmentStub();

            compiler = new CSharpCompilerTask(fileSystem, environment);
        }

        [Test]
    	public void Should_support_Optimize_flag()
    	{
			compiler.Optimize = true;
			environment.RunHandler = (fileName, arguments, x) =>
			{
				arguments.Contains(" /optimize+").ShouldBe(true);
			};
			compiler.Output = new Path("MyAssembly.dll");
			compiler.Execute();
    	}

        [TestCase(CompilerVersion.v35, "v3.5")]
        [TestCase(CompilerVersion.v40, "v4.0.30319")]
        public void Should_support_compiler_version(CompilerVersion version, string fxVersionFolder)
        {
            compiler.Version = version;

            fileSystem.GetDirectoriesHandler = (root, pattern) => new[] { Path.Empty + fxVersionFolder };

            environment.RunHandler = (fileName, arguments, x) =>
            {
                fileName.Contains(fxVersionFolder).ShouldBe(true);
            };

            compiler.Output = new Path("MyAssembly.dll");
            compiler.Execute();
        }

        [Test]
        public void Should_create_target_directory_if_not_present()
        {
			var outDir = new Path("Build").Combine("Debug");
            compiler.Output = outDir.Combine("Pencil.Build.dll");
            Path createdDirectory = Path.Empty;

            fileSystem.DirectoryExistsHandler = x => false;
            fileSystem.CreateDirectoryHandler = x => createdDirectory = new Path(x);
            compiler.Execute();

            outDir.ShouldEqual(createdDirectory);
        }

        [Test]
        public void Should_copy_referenced_assemblies()
        {
			var outDir = new Path("Build");
            compiler.Output = outDir.Combine("Bar.dll");
            compiler.References.Add(new Path("Foo.dll"));

            fileSystem.DirectoryExistsHandler = x => true;
            fileSystem.FileExistsHandler = x => x.Equals(new Path("Foo.dll"));
            bool copied = false;
            fileSystem.CopyFileHandler = (from, to, overwrite) =>
            {
                Assert.AreEqual(new Path("Foo.dll"), from);
                Assert.AreEqual(outDir + "Foo.dll", to);
                copied = true;
            };
            compiler.Execute();

            Assert.IsTrue(copied, "Referenced assembly not copied.");
        }

        [Test]
        public void Wont_copy_referenced_assemblies_already_present()
        {
            compiler.Output = new Path("Build").Combine("Bar.dll");
            compiler.References.Add(new Path("Foo.dll"));

            fileSystem.DirectoryExistsHandler = x => true;
            fileSystem.FileExistsHandler = x => true;
            fileSystem.CopyFileHandler = (from, to, overwrite) =>
            {
                Assert.Fail("Should not try to copy file already present.");
            };
            compiler.Execute();
        }
    }
}
