using System.Collections.Generic;

namespace Pencil.Build
{
    public abstract class Target
	{
		public void Execute()
		{
			SatisfyDependencies();
			ExecuteCore();
		}

		private void SatisfyDependencies()
		{
			foreach(var item in GetDependencies())
				Project.Run(item);
		}

		public virtual IEnumerable<string> GetDependencies()
		{
			return new string[0];
		}

        protected abstract IProject Project { get; }

        protected abstract void ExecuteCore();

        public abstract bool IsDefault { get; }

        public abstract string Name { get; }
	}
}