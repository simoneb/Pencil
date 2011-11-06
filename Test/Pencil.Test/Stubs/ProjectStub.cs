using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using OpenFileSystem.IO;

namespace Pencil.Test.Stubs
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
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

        public IEnumerable<Target> Targets
        {
            get { throw new NotImplementedException(); }
        }

        public ILogger Logger
        {
            get { return Pencil.Logger.Null; }
        }

        public IList<string> ReferencedAssemblies
        {
            get { return new List<string>(); }
        }

        public void DisplayTargets(Logger logger)
        {
            throw new NotImplementedException();
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