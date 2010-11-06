using System;
using System.Linq;
using OpenFileSystem.IO.FileSystem.Local;

namespace Pencil
{
	public class Program
	{
		public const int Success = 0;
		public const int Failure = 1;

		readonly Logger logger;
		readonly Converter<string,IProject> compiler;

 		public Program(Logger logger, Converter<string, IProject> compiler)
		{
			this.logger = logger;
			this.compiler = compiler;
		}

		public int Run(string[] args)
		{
			var project = compiler(args[0]);

			project.Register(LocalFileSystem.Instance);
			project.Register<IExecutionEnvironment>(new ExecutionEnvironment(logger));

            if (project.HasDefaultTarget && args.Count() == 1)
                BuildTarget(project, project.DefaultTarget);
            else
		        foreach (var target in args.Skip(1))
		            if (BuildTarget(project, target) != Success)
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