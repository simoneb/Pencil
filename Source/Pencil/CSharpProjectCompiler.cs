using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Linq;
using Microsoft.CSharp;

namespace Pencil
{
    class CSharpProjectCompiler : IProjectCompiler
    {
		readonly CodeDomProvider codeProvider;
		readonly ILogger logger;
		readonly IEnumerable<string> referencedAssemblies;

		public CSharpProjectCompiler(ILogger logger, IEnumerable<string> referencedAssemblies, CompilerVersion compilerVersion)
		{
			this.logger = logger;
			codeProvider = new CSharpCodeProvider(new Dictionary<string, string> {{"CompilerVersion", compilerVersion.CodePoviderName}});
			this.referencedAssemblies = referencedAssemblies.Union(DefaultAssemblies);
		}
        
        private static IEnumerable<string> DefaultAssemblies
        {
            get
            {
                yield return Assembly.GetExecutingAssembly().Location;
                yield return "System.dll";
            }
        }

		public IProject Compile(string path)
		{
			var result = codeProvider.CompileAssemblyFromFile(GetCompilerParameters(), path);

			if(result.NativeCompilerReturnValue == 0)
				return GetProject(result.CompiledAssembly);

			throw new CompilationFailedException(result);
		}

		private CompilerParameters GetCompilerParameters()
		{
			var options = new CompilerParameters {GenerateExecutable = false, GenerateInMemory = true};

            options.ReferencedAssemblies.AddRange(referencedAssemblies.ToArray());

		    return options;
		}

	    private IProject GetProject(Assembly assembly)
		{
			foreach(var item in assembly.GetExportedTypes())
				if(typeof(Project).IsAssignableFrom(item))
				{
					var project = (Project)item.GetConstructor(Type.EmptyTypes).Invoke(null);
					project.Logger = logger;

				    foreach (var a in referencedAssemblies)
				        project.ReferencedAssemblies.Add(a);

				    return project;
				}

			throw new InvalidOperationException(string.Format("{0} does not contain any Project.", assembly));
		}
	}
}