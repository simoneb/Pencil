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
			var logger = new Logger(Console.Out);
			var codeProvider = new CSharpCodeProvider(new Dictionary<string,string> {{"CompilerVersion", "v3.5"}});
			var compiler = new ProjectCompiler(logger, codeProvider, GetReferencedAssemblies(args));
			var program = new Program(logger, compiler.ProjectFromFile);

			program.ShowLogo();

			var stopwatch = Stopwatch.StartNew();

            try
            {
				return program.Run(GetArguments(args));
            }
            finally
            {
                stopwatch.Stop();
                logger.Write("Total time: {0} seconds.", stopwatch.Elapsed.Seconds);
            }
		}

		public static IEnumerable<Path> GetReferencedAssemblies(IEnumerable<string> args)
		{
			yield return new Path(Assembly.GetExecutingAssembly().Location);
			yield return new Path("System.dll");

            foreach(var item in args)
				if(item.StartsWith("-r:"))
					yield return new Path(item.Substring("-r:".Length));
		}

		public static string[] GetArguments(IEnumerable<string> args)
		{
			return new List<string>(args.Where(x => !x.StartsWith("-"))).ToArray();
		}
	}
}