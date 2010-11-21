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
		readonly Logger logger;
		readonly IEnumerable<string> referencedAssemblies;

		public CSharpProjectCompiler(Logger logger, IEnumerable<string> referencedAssemblies, CompilerVersion compilerVersion)
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
				if(typeof(IProject).IsAssignableFrom(item))
				{
					var project = (Project)item.GetConstructor(Type.EmptyTypes).Invoke(null);
					project.Logger = logger;
				    project.ReferencedAssemblies.AddRange(referencedAssemblies);
					return project;
				}

			throw new InvalidOperationException(string.Format("{0} does not contain any Project.", assembly));
		}
	}
}