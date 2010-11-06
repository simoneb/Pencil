using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Pencil.Build
{
    public class MethodTarget : Target
    {
        private readonly IProject project;
        private readonly MethodInfo method;

        public MethodTarget(IProject project, MethodInfo method)
        {
            this.project = project;
            this.method = method;
        }

        public override IEnumerable<string> GetDependencies()
        {
            return from DependsOnAttribute item
                       in method.GetCustomAttributes(typeof (DependsOnAttribute), false)
                   select item.Name;
        }

        protected override IProject Project
        {
            get { return project; }
        }

        protected override void ExecuteCore()
        {
            try
            {
                method.Invoke(project, null);
            }
            catch (TargetInvocationException e)
            {
                throw new TargetFailedException(e.InnerException);
            }
        }

        public override bool IsDefault
        {
            get { return method.IsDefined(typeof (DefaultAttribute), false); }
        }
    }
}