namespace Pencil.Test.Stubs
{
    using System.Collections.Generic;
    using System;
    using Pencil.Build;

    class TargetStub : Target
    {
		IProject project;
        IEnumerable<string> dependencies;

        public Action ExecuteHandler;

        public TargetStub(IProject project, IEnumerable<string> dependencies)
        {
			this.project = project;
            this.dependencies = dependencies;
        }

        public override IEnumerable<string> GetDependencies()
        {
            return dependencies;
        }

        protected override IProject Project
        {
            get { return project; }
        }

        protected override void ExecuteCore() { ExecuteHandler(); }

        public override bool IsDefault
        {
            get { throw new NotImplementedException(); }
        }
    }
}