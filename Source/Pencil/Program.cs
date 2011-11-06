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
		readonly IProjectCompiler compiler;

 		public Program(Logger logger, IProjectCompiler compiler)
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

            if(string.IsNullOrEmpty(options.BuildScript))
            {
                options.Display(logger);
                return Success;
            }

		    var project = compiler.Compile(options.BuildScript);

		    if(options.ShowTargets)
		    {
		        project.DisplayTargets(logger);
                return Success;
            }

		    return Run(options, project);
		}

	    private int Run(IPencilOptions options, IProject project)
	    {
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

	        logger.WriteLine("BUILD SUCCEEDED");

	        return Success;
	    }

	    internal int BuildTarget(IProject project, string target)
		{
            try
            {
                if(project.HasTarget(target))
                {
                    project.Run(target);
                    return Success;
                }
                
                logger.WriteLine("Target \"{0}\" not found.", target);
            }
            catch(TargetFailedException e)
            {
				var error = e.InnerException;
                logger.WriteLine("BUILD FAILED - {0}", error.Message);
				logger.WriteLine(error.StackTrace);
            }

            return Failure;
        }

	    private void ShowLogo()
		{
	        var assemblyName = GetType().Assembly.GetName();
            logger.WriteLine("{0} version {1}", assemblyName.Name, assemblyName.Version);
			logger.WriteLine("Copyright (C) 2008 Torbjörn Gyllebring");
			logger.WriteLine("Copyright (C) 2010 Simone Busoli");
            logger.WriteLine();
		}
	}
}