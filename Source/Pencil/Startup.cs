using System.Linq;
using System.Reflection;
using OpenFileSystem.IO.FileSystem.Local;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.CSharp;

namespace Pencil
{
    public static class Startup
	{
		static int Main(string[] args)
		{
		    var parser = new PencilOptionsParser();
		    var result = parser.Parse(args);
		    var logger = new Logger(Console.Out);
		    var codeProvider = new CSharpCodeProvider(new Dictionary<string, string> {{"CompilerVersion", "v3.5"}});
		    var compiler = new ProjectCompiler(logger, codeProvider, result.Assemblies.Union(GetDefaultAssemblies()));
			var program = new Program(logger, compiler.ProjectFromFile);

			var stopwatch = Stopwatch.StartNew();

            try
            {
				return program.Run(result);
            }
            finally
            {
                stopwatch.Stop();
                logger.Write("Total time: {0} seconds.", stopwatch.Elapsed.Seconds);
            }
		}

        private static IEnumerable<Path> GetDefaultAssemblies()
		{
			yield return new Path(Assembly.GetExecutingAssembly().Location);
			yield return new Path("System.dll");
		}
	}
}