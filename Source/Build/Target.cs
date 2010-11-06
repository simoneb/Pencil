namespace Pencil.Build
{
	using System.Collections.Generic;

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

        protected virtual void ExecuteCore(){}

        public abstract bool IsDefault { get; }
	}
}