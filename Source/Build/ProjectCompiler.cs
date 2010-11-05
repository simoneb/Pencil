using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using OpenFileSystem.IO.FileSystem.Local;
using Pencil.Core;

namespace Pencil.Build
{
	class ProjectCompiler
	{
		readonly CodeDomProvider codeProvider;
		readonly Logger logger;
		readonly IEnumerable<Path> referencedAssemblies;

		public ProjectCompiler(Logger logger, CodeDomProvider codeProvider,
			IEnumerable<Path> referencedAssemblies)
		{
			this.logger = logger;
			this.codeProvider = codeProvider;
			this.referencedAssemblies = referencedAssemblies;
		}

		public IProject ProjectFromFile(string path)
		{
			var result = codeProvider.CompileAssemblyFromFile(GetCompilerParameters(), path);
			if(result.NativeCompilerReturnValue == 0)
				return GetProject(result.CompiledAssembly.GetTypes());
			throw new CompilationFailedException(result);
		}

		CompilerParameters GetCompilerParameters()
		{
			var options = new CompilerParameters {GenerateExecutable = false, GenerateInMemory = true};
		    referencedAssemblies.ForEach(x => options.ReferencedAssemblies.Add(x.ToString()));
			return options;
		}

		IProject GetProject(IEnumerable<Type> types)
		{
			foreach(var item in types)
				if(typeof(IProject).IsAssignableFrom(item))
				{
					var project = item.GetConstructor(Type.EmptyTypes).Invoke(null) as Project;
					project.logger = logger;
					return project;
				}
			throw new InvalidOperationException(string.Format("{0} does not contain any Project."));
		}
	}
}