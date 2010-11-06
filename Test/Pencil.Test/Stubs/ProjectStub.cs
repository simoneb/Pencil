using OpenFileSystem.IO;

namespace Pencil.Test.Stubs
{
    using System;

    class ProjectStub : IProject
    {
        public Action<string> RunHandler = x => {};
        public Func<IFileSystem> FileSystemHandler = () => null;
        public Func<IExecutionEnvironment> PlatformHandler = () => null;
		public Predicate<string> HasTargetHandler;
		public Func<IProject, string> DefaultTargetHandler = x => null;

        public bool HasTarget(string name) { return HasTargetHandler(name); }
        public void Run(string target) { RunHandler(target); }
		public void Register<T>(T instance){}

        public bool HasDefaultTarget
        {
            get { return !string.IsNullOrEmpty(DefaultTarget); }
        }

        public string DefaultTarget
        {
            get { return DefaultTargetHandler(this); }
        }

        public IFileSystem FileSystem
        {
            get { return FileSystemHandler(); }
        }

        public IExecutionEnvironment Platform
        {
            get { return PlatformHandler(); }
        }
    }
}