using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Linq;

namespace Pencil
{
	class ProjectCompiler
	{
		readonly CodeDomProvider codeProvider;
		readonly Logger logger;
		readonly IEnumerable<string> referencedAssemblies;

		public ProjectCompiler(Logger logger, CodeDomProvider codeProvider, IEnumerable<string> referencedAssemblies)
		{
			this.logger = logger;
			this.codeProvider = codeProvider;
			this.referencedAssemblies = referencedAssemblies;
		}

		public IProject ProjectFromFile(string path)
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
					return project;
				}

			throw new InvalidOperationException(string.Format("{0} does not contain any Project.", assembly));
		}
	}
}