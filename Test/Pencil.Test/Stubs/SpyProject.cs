using System;

namespace Pencil.Test.Stubs
{
    public class SpyProject : Project
    {
        public Action<Target> RunHandler = ignored => {};

        protected override void RunCore(Target target)
        {
            RunHandler(target);
            base.RunCore(target);
        }
    }
}