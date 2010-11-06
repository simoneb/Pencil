using System;
using System.Linq;

namespace Pencil.Build
{
	using System.Collections.Generic;

    static class MethodTargetExtractor
	{
		public static Dictionary<string, Target> GetTargets(IProject project)
		{
			var targets = new Dictionary<string, Target>(StringComparer.OrdinalIgnoreCase);
			
            foreach (var m in project.GetType().GetMethods().Where(m => m.DeclaringType != typeof (object)))
            {
                try
                {
                    targets.Add(m.Name, new MethodTarget(project, m));
                }
                catch (ArgumentException e)
                {
                    var existingTarget = targets[m.Name].Name;
                    throw new DuplicateTargetException(existingTarget, m.Name, e);
                }
            }

		    return targets;
		}
	}
}