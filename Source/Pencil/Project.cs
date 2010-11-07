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
		readonly Dictionary<string, Target> targets;
		readonly HashSet<string> done = new HashSet<string>();
		internal Logger Logger = new Logger(TextWriter.Null);
		readonly ZeptoContainer container = new ZeptoContainer();

		public Project()
		{
			targets = MethodTargetExtractor.GetTargets(this);
		}

	    public string DefaultTarget
	    {
            get { return targets.FirstOrDefault(t => t.Value.IsDefault).Key; }
	    }

        public IFileSystem FileSystem { get { return New<IFileSystem>(); } }
        public IExecutionEnvironment Platform { get { return New<IExecutionEnvironment>(); } }

	    public T New<T>()
		{
			return container.Get<T>();
		}

		public bool HasTarget(string name)
		{
			return targets.ContainsKey(name);
		}

		public void Run(string targetName)
		{
			if(done.Contains(targetName))
				return;

		    var target = targets[targetName];
		    var realTargetName = target.Name;

		    Logger.Write("{0}:", realTargetName);

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

        protected void Call(Action target)
        {
            Run(MethodTarget.GetTargetName(target.Method));
        }
	}
}