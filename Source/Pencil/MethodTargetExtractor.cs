using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Pencil
{
    public static class MethodTargetExtractor
	{
		public static Dictionary<string, Target> GetTargets(IProject project)
		{
			var targets = new Dictionary<string, Target>(StringComparer.OrdinalIgnoreCase);
			
            foreach (var m in GetSuitableMethods(project))
            {
                try
                {
                    targets.Add(MethodTarget.GetTargetName(m), new MethodTarget(project, m));
                }
                catch (ArgumentException e)
                {
                    var existingTarget = targets[m.Name].Name;
                    throw new DuplicateTargetException(existingTarget, m.Name, e);
                }
            }

		    return targets;
		}

        private static IEnumerable<MethodInfo> GetSuitableMethods(IProject project)
        {
            var typesToExclude = new[] {typeof (Project), typeof (IProject), typeof (Object)};

            return from method in project.GetType().GetMethods()
                   where typesToExclude.None(t => t.Equals(method.DeclaringType))
                   where !method.IsSpecialName
                   select method;
        }
	}
}