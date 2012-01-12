using System.IO;
using Mono.Options;
using System;
using System.Diagnostics;

namespace Pencil
{
    public static class Startup
	{
		static int Main(string[] args)
		{
		    var logger = new Logger(Console.Out);
		    var parser = new PencilOptionsParser();

		    IPencilOptions options;

		    try
		    {
		        options = parser.Parse(args);
		    }
		    catch (OptionException e)
		    {
		        logger.WriteLine(e.Message);
                parser.Display(logger);
		        return Program.Failure;
		    }

		    var compiler = GetCompiler(options, logger);
			var program = new Program(logger, compiler);

			var stopwatch = Stopwatch.StartNew();

            try
            {
				return program.Run(options);
            }
            finally
            {
                stopwatch.Stop();
                logger.WriteLine("Total time: {0} seconds.", stopwatch.Elapsed.Seconds);
            }
		}

        private static IProjectCompiler GetCompiler(IPencilOptions options, Logger logger)
        {

            switch (Path.GetExtension(options.BuildScript))
            {
                case ".cs":
                    return new CSharpProjectCompiler(logger, options.Assemblies, options.CompilerVersion);
                case ".js":
                    return new JScriptProjectCompiler(logger, options.Assemblies);
                default:
                    throw new NotSupportedException(options.BuildScript);
            }
        }
	}
}