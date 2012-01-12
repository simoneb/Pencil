using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pencil
{
    internal abstract class AbstractProjectCompiler : IProjectCompiler
    {
        protected readonly ILogger Logger;
        protected readonly IEnumerable<string> ReferencedAssemblies;

        protected AbstractProjectCompiler(ILogger logger, IEnumerable<string> referencedAssemblies)
        {
            Logger = logger;
            ReferencedAssemblies = referencedAssemblies.Union(DefaultAssemblies);
        }

        private static IEnumerable<string> DefaultAssemblies
        {
            get
            {
                yield return Assembly.GetExecutingAssembly().Location;
                yield return "System.dll";
            }
        }

        protected IProject GetProject(Assembly assembly)
        {
            foreach(var item in assembly.GetExportedTypes())
                if(typeof(Project).IsAssignableFrom(item))
                {
                    var project = (Project)item.GetConstructor(Type.EmptyTypes).Invoke(null);
                    project.Logger = Logger;

                    foreach (var a in ReferencedAssemblies)
                        project.ReferencedAssemblies.Add(a);

                    return project;
                }

            throw new InvalidOperationException(string.Format("{0} does not contain any Project.", assembly));
        }

        public abstract IProject Compile(string scriptPath);
    }
}