using OpenFileSystem.IO.FileSystem.InMemory;
using OpenFileSystem.IO.FileSystem.Local;

namespace Pencil.Test.Build.Tasks
{
	using System;
	using NUnit.Framework;
	using Pencil.Build;
	using Pencil.Build.Tasks;
	using Pencil.Test.Stubs;
	using Pencil.IO;

	[TestFixture]
	public class NUnitTaskTests
	{
		[Test]
		public void Should_use_mono_for_program_if_running_on_mono()
		{
		    var fileSystem = new InMemoryFileSystem();
			var envionrment = new ExecutionEnvironmentStub();
			envionrment.IsMonoHandler = () => true;
			var nunit = new NUnitTask(fileSystem, envionrment);

			nunit.Program.ShouldEqual(new Path("mono"));
		}
		[Test]
		public void Should_use_nunit_console_for_program_if_not_on_mono()
		{
            var fileSystem = new InMemoryFileSystem();
            var envionrment = new ExecutionEnvironmentStub();
			envionrment.IsMonoHandler = () => false;
			var nunit = new NUnitTask(fileSystem, envionrment);

			nunit.Program.ToString().EndsWith("nunit-console.exe").ShouldBe(true);
		}
		[Test]
		public void NUnitPath_should_be_based_on_NUnitBinPath()
		{
            var fileSystem = new InMemoryFileSystem();
            var envionrment = new ExecutionEnvironmentStub();
			envionrment.IsMonoHandler = () => false;
			var nunit = new NUnitTask(fileSystem, envionrment);
			var binPath = new Path("NUnit").Combine("bin");
			nunit.NUnitBinPath = binPath;
			nunit.NUnitPath.ShouldEqual(binPath.Combine("nunit-console.exe"));
		}
		[Test]
		public void Arguments_should_start_with_NUnitPath_on_mono()
		{
            var fileSystem = new InMemoryFileSystem();
			var environment = new ExecutionEnvironmentStub();
			environment.IsMonoHandler = () => true;
			var nunit = new NUnitTask(fileSystem, environment);

			environment.RunHandler += (p, args, x) => 
			{
				args.StartsWith(nunit.NUnitPath.ToString()).ShouldBe(true);
			};
			nunit.Execute();
		}
		[Test]
		public void Arguments_should_contain_Target()
		{
            var fileSystem = new InMemoryFileSystem();
            var environment = new ExecutionEnvironmentStub();
			var nunit = new NUnitTask(fileSystem, environment);
			nunit.Target = new Path("MyTests.dll");
			environment.RunHandler += (p, args, x) => 
			{
				args.Contains("MyTests.dll").ShouldBe(true);
			};
			nunit.Execute();
		}
		[Test]
		public void Should_support_disabling_shadow_copy()
		{
            var fileSystem = new InMemoryFileSystem();
            var environment = new ExecutionEnvironmentStub();
			var nunit = new NUnitTask(fileSystem, environment);
			nunit.ShadowCopy = false;
			environment.RunHandler += (p, args, x) => 
			{
				args.Contains("-noshadow").ShouldBe(true);
			};
			nunit.Execute();
		}
		[Test]
		public void Should_support_disabling_logo()
		{
            var fileSystem = new InMemoryFileSystem();
            var environment = new ExecutionEnvironmentStub();
			var nunit = new NUnitTask(fileSystem, environment);
			nunit.ShowLogo = false;
			environment.RunHandler = (p, args, x) => 
			{
				args.Contains("-nologo").ShouldBe(true);
			};
			nunit.Execute();
		}
	}
}
