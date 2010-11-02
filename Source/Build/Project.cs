using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Pencil.Build
{
	public class Project : IProject
	{
		readonly Dictionary<string, Target> targets;
		readonly HashSet<string> done = new HashSet<string>();
		internal Logger logger = new Logger(TextWriter.Null);
		readonly ZeptoContainer container = new ZeptoContainer();

		public Project()
		{
			targets = MethodTargetExtractor.GetTargets(this);
		}

	    public string DefaultTarget
	    {
            get { return targets.FirstOrDefault(t => t.Value.IsDefault).Key; }
	    }

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

			logger.Write("{0}:", targetName);

			using(logger.Indent())
			{
				RunCore(targetName);
				done.Add(targetName);
			}
		}

		protected virtual void RunCore(string targetName)
		{
			targets[targetName].Execute();
		}

		public void Register<T>(T instance){ container.Register(typeof(T), instance); }

	    public bool HasDefaultTarget
	    {
            get { return !string.IsNullOrEmpty(DefaultTarget); }
	    }
	}
}