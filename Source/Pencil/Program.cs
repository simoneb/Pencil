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

		public int Run(string[] args)
		{
			var project = compiler(args[0]);

		    var fileSystem = LocalFileSystem.Instance;
		    var platform = new ExecutionEnvironment(logger);

		    project.Register(fileSystem);
		    project.Register<IExecutionEnvironment>(platform);

		    var buildFilePath = fileSystem.GetFile(args[0]).Parent.Path.ToString();

            using (Pushd(buildFilePath, platform))
                return Run(args, project);
		}

	    private static IDisposable Pushd(string directory, IExecutionEnvironment platform)
	    {
	        var current = platform.CurrentDirectory;
		    platform.CurrentDirectory = directory;

	        return new DisposableAction(() => platform.CurrentDirectory = current);
	    }

	    private int Run(IEnumerable<string> args, IProject project)
	    {
            if (project.HasDefaultTarget && args.Count() == 1)
            {
                if (BuildTarget(project, project.DefaultTarget) != Success)
                    return Failure;
            }
            else if (args.Skip(1).Any(target => BuildTarget(project, target) != Success))
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

		public void ShowLogo()
		{
			logger.Write("Pencil.Build version {0}", GetType().Assembly.GetName().Version);
			logger.Write("Copyright (C) 2008 Torbjörn Gyllebring");
		}
	}
}