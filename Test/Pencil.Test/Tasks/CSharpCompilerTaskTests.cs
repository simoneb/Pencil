using System.Runtime.InteropServices;
using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.InMemory;
using OpenFileSystem.IO.FileSystem.Local;
using NUnit.Framework;
using Pencil.Tasks;
using Pencil.Test.Stubs;

namespace Pencil.Test.Tasks
{
    [TestFixture]
    public class CSharpCompilerTaskTests
    {
        private CSharpCompilerTask compiler;
        private ExecutionEnvironmentStub environment;
        private InMemoryFileSystem fileSystem;

        [SetUp]
        public void Setup()
        {
            fileSystem = new InMemoryFileSystem();
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

        [TestCase("v3.5", "v3.5")]
        [TestCase("v4.0", "v4.0.30319")]
        public void Should_support_compiler_version(string version, string fxVersionFolder)
        {
            compiler.Version = CompilerVersion.FromName(version);

            fileSystem.GetDirectory(RuntimeEnvironment.GetRuntimeDirectory()).Parent.Create().GetOrCreateDirectory(fxVersionFolder);

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
            compiler.Execute();

            fileSystem.GetDirectory(outDir.FullPath).Exists.ShouldBe(true);
        }

        [Test]
        public void Should_copy_referenced_assemblies()
        {
			var outDir = new Path("Build");
            compiler.Output = outDir.Combine("Bar.dll");
            compiler.References.Add(new Path("Foo.dll"));

            fileSystem.GetDirectory(outDir.FullPath).MustExist();
            fileSystem.GetFile("Foo.dll").MustExist();
            compiler.Execute();

            Assert.IsTrue(fileSystem.GetFile(outDir.Combine("Foo.dll").FullPath).Exists);
        }

        [Test]
        [Ignore("Can't run now with OpenFileSystem")]
        public void Wont_copy_referenced_assemblies_already_present()
        {
            compiler.Output = new Path(@"Build\Bar.dll");
            compiler.References.Add(new Path("Foo.dll"));

            fileSystem.GetFile(@"Build\Foo.dll").MustExist();

            //fileSystem.DirectoryExistsHandler = x => true;
            //fileSystem.FileExistsHandler = x => true;
            //fileSystem.CopyFileHandler = (from, to, overwrite) =>
            //{
            //    Assert.Fail("Should not try to copy file already present.");
            //};
            compiler.Execute();
        }
    }
}
