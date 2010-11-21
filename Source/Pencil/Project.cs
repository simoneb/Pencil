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
        private ICollection<FutureProject> includes = new List<FutureProject>();

        public Project()
		{
			targets = MethodTargetExtractor.GetTargets(this);
		}

	    public string DefaultTarget
	    {
            get { return targets.FirstOrDefault(t => t.Value.IsDefault).Key; }
	    }

        public IEnumerable<Target> Targets
        {
            get { return targets.Values; }
        }

        public void DisplayTargets(Logger logger)
        {
            var start = targets.Keys.Select(t => t.Length).OrderByDescending(x => x).First();

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
			return targets.ContainsKey(name);
		}

		public void Run(string targetName)
		{
			if(done.Contains(targetName))
				return;

		    var target = targets[targetName];
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

        public List<string> ReferencedAssemblies = new List<string>();

        protected void Call(Action target)
        {
            Run(MethodTarget.GetTargetName(target.Method));
        }

        protected void Pencil(string buildScript, string target){}

        protected void Include(string file)
        {
            if(!File.Exists(file))
                throw new InvalidOperationException();

            includes.Add(new FutureProject(this, file));
        }

        protected T Property<T>(string name)
        {
            foreach (var futureProject in includes)
            {
                var propertyInfo = futureProject.Value.GetType().GetProperty(name, typeof(T));
        
                if (propertyInfo != null)
                    return (T) propertyInfo.GetValue(futureProject.Value, null);
            }

            return default(T);
        }
	}
}