using System;
using System.Linq;
using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;

namespace Pencil.Build
{
	public class Program
	{
		public const int Success = 0;
		public const int Failure = 1;

		readonly Logger output;
		readonly Converter<string,IProject> compiler;

 		public Program(Logger output, Converter<string, IProject> compiler)
		{
			this.output = output;
			this.compiler = compiler;
		}

		public int Run(string[] args)
		{
			var project = compiler(args[0]);

			project.Register<IFileSystem>(LocalFileSystem.Instance);
			project.Register<IExecutionEnvironment>(new ExecutionEnvironment(output.Target));

            if (project.HasDefaultTarget && args.Count() == 1)
                BuildTarget(project, project.DefaultTarget);
            else
		        foreach (var target in args.Skip(1))
		            if (BuildTarget(project, target) != Success)
		                return Failure;

		    output.Write("BUILD SUCCEEDED");

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
                
                output.Write("Target \"{0}\" not found.", target);
            }
            catch(TargetFailedException e)
            {
				var error = e.InnerException;
                output.Write("BUILD FAILED - {0}", error.Message);
				output.Write(error.StackTrace);
            }

            return Failure;
        }

		public void ShowLogo()
		{
			output.Write("Pencil.Build version {0}", GetType().Assembly.GetName().Version);
			output.Write("Copyright (C) 2008 Torbjörn Gyllebring");
		}
	}
}