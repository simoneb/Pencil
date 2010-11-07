using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Pencil.Attributes;

namespace Pencil
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

        public override string Name
        {
            get { return method.Name; }
        }

        public override string Description
        {
            get
            {
                return method.GetCustomAttributes(typeof (DescriptionAttribute), false)
                           .Cast<DescriptionAttribute>()
                           .SingleOrDefault().Get(x => x.Description) ?? string.Empty;
            }
        }

        public static string GetTargetName(MethodInfo method)
        {
            return method.Name;
        }
    }
}