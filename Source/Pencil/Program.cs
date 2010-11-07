using System;
using System.Collections.Generic;
using System.Linq;
using OpenFileSystem.IO.FileSystem.Local;

namespace Pencil
{
	public class Program
	{
		public const int Success = 0;
		public const int Failure = 1;

		readonly Logger logger;
		readonly Converter<string, IProject> compiler;

 		public Program(Logger logger, Converter<string, IProject> compiler)
		{
			this.logger = logger;
			this.compiler = compiler;
		}

		public int Run(IPencilOptions options)
		{
            if(!options.NoLogo)
                ShowLogo();

            if(options.Help)
            {
                options.Display(logger);
                return Success;
            }

			var project = compiler(options.BuildScript);

		    var fileSystem = LocalFileSystem.Instance;
		    var platform = new ExecutionEnvironment(logger);

		    project.Register(fileSystem);
		    project.Register<IExecutionEnvironment>(platform);

		    var buildFilePath = fileSystem.GetFile(options.BuildScript).Parent.Path.ToString();

            using (Pushd(buildFilePath, platform))
                return Run(options.Targets, project);
		}

	    private static IDisposable Pushd(string directory, IExecutionEnvironment platform)
	    {
	        var current = platform.CurrentDirectory;
		    platform.CurrentDirectory = directory;

	        return new DisposableAction(() => platform.CurrentDirectory = current);
	    }

	    private int Run(IEnumerable<string> targets, IProject project)
	    {
            if (targets.Empty() && project.HasDefaultTarget)
            {
                if (BuildTarget(project, project.DefaultTarget) != Success)
                    return Failure;
            }
            else if (targets.Any(target => BuildTarget(project, target) != Success))
                return Failure;

	        logger.Write("BUILD SUCCEEDED");

	        return Success;
	    }

	    public int BuildTarget(IProject project, string target)
		{
            try
            {
                if(project.HasTarget(target))
                {
                    project.Run(target);
                    return Success;
                }
                
                logger.Write("Target \"{0}\" not found.", target);
            }
            catch(TargetFailedException e)
            {
				var error = e.InnerException;
                logger.Write("BUILD FAILED - {0}", error.Message);
				logger.Write(error.StackTrace);
            }

            return Failure;
        }

	    private void ShowLogo()
		{
			logger.Write("Pencil.Build version {0}", GetType().Assembly.GetName().Version);
			logger.Write("Copyright (C) 2008 Torbjörn Gyllebring");
			logger.Write("Copyright (C) 2010 Simone Busoli");
            logger.WriteLine();
		}
	}
}