using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Linq;
using Microsoft.CSharp;

namespace Pencil
{
    class CSharpProjectCompiler : AbstractProjectCompiler
    {
		readonly CodeDomProvider codeProvider;

        public CSharpProjectCompiler(ILogger logger, IEnumerable<string> referencedAssemblies, CompilerVersion compilerVersion) : base(logger, referencedAssemblies)
		{
			codeProvider = new CSharpCodeProvider(new Dictionary<string, string> {{"CompilerVersion", compilerVersion.CodePoviderName}});
		}

		public override IProject Compile(string scriptPath)
		{
			var result = codeProvider.CompileAssemblyFromFile(GetCompilerParameters(), scriptPath);

			if(result.NativeCompilerReturnValue == 0)
				return GetProject(result.CompiledAssembly);

			throw new CompilationFailedException(result);
		}

		private CompilerParameters GetCompilerParameters()
		{
			var options = new CompilerParameters {GenerateExecutable = false, GenerateInMemory = true};

            options.ReferencedAssemblies.AddRange(ReferencedAssemblies.ToArray());

		    return options;
		}
    }
}