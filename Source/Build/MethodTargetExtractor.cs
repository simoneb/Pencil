namespace Pencil.Build
{
	using System.Collections.Generic;

    static class MethodTargetExtractor
	{
		public static Dictionary<string, Target> GetTargets(IProject project)
		{
			var targets = new Dictionary<string, Target>();
			
            foreach(var m in project.GetType().GetMethods())
			    if(m.DeclaringType != typeof(object))
				    targets.Add(m.Name, new MethodTarget(project, m));
			
            return targets;
		}
	}
}