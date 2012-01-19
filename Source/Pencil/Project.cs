using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using OpenFileSystem.IO;

namespace Pencil
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
	public class Project : IProject
	{
		readonly Dictionary<string, Target> targetsByName;
		readonly HashSet<string> done = new HashSet<string>();
        readonly ZeptoContainer container = new ZeptoContainer();

        public Project()
        {
            Logger = new Logger(TextWriter.Null);
            targetsByName = MethodTargetExtractor.GetTargets(this);
        }

        public string DefaultTarget
	    {
            get { return targetsByName.FirstOrDefault(t => t.Value.IsDefault).Key; }
	    }

        public IEnumerable<Target> Targets
        {
            get { return targetsByName.Values; }
        }

        public void DisplayTargets(Logger logger)
        {
            var start = targetsByName.Keys.Select(t => t.Length).OrderByDescending(x => x).First();

            foreach (var target in Targets)
            {
                logger.Write("- {0}", target.Name, target.Description);
                logger.WriteLine(!string.IsNullOrEmpty(target.Description) 
                    ? new string(' ', start - target.Name.Length + 2) + "# " + target.Description 
                    : string.Empty);
            }
        }

        public IFileSystem FileSystem { get { return New<IFileSystem>(); } }
        public IExecutionEnvironment Platform { get { return New<IExecutionEnvironment>(); } }

	    public T New<T>()
		{
			return container.Get<T>();
		}

		public bool HasTarget(string name)
		{
			return targetsByName.ContainsKey(name);
		}

		public void Run(string targetName)
		{
			if(done.Contains(targetName))
				return;

		    var target = targetsByName[targetName];
		    var realTargetName = target.Name;

		    Logger.WriteLine("{0}:", realTargetName);

			using(Logger.Indent())
			{
                RunCore(target);
				done.Add(targetName);
			}
		}

        protected virtual void RunCore(Target target)
		{
			target.Execute();
		}

		public void Register<T>(T instance){ container.Register(typeof(T), instance); }

	    public bool HasDefaultTarget
	    {
            get { return !string.IsNullOrEmpty(DefaultTarget); }
	    }

        public ILogger Logger { get; set; }

        public IList<string> ReferencedAssemblies
        {
            get { return referencedAssemblies; }
        }

        private List<string> referencedAssemblies = new List<string>();

        protected void Call(Action target)
        {
            Run(MethodTarget.GetTargetName(target.Method));
        }
	}
}