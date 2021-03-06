using System.IO;
using OpenFileSystem.IO;
using System;
using System.Runtime.InteropServices;
using Path = OpenFileSystem.IO.FileSystem.Local.Path;

namespace Pencil.Tasks
{
    public abstract class ExecTaskBase
    {
        protected IFileSystem FileSystem { get; private set; }
        protected IExecutionEnvironment Platform { get; private set; }

        protected ExecTaskBase() : this(ProjectContext.FileSystem, ProjectContext.ExecutionEnvironment)
        {
        }

        internal ExecTaskBase(IFileSystem fileSystem, IExecutionEnvironment platform)
        {
            FileSystem = fileSystem;
            Platform = platform;
        }

        public bool ShowCommandLine { get; set; }

        public void Run()
        {
            var fileName = Program.ToString();
            var arguments = GetArguments();

            if(ShowCommandLine)
            {
                Platform.Logger.WriteLine("Starting {0} {1}", fileName, arguments);
                Platform.Logger.WriteLine("in directory {0}", Platform.CurrentDirectory);
            }

            Platform.Run(fileName, arguments, task =>
                                              {
                                                  while (!task.HasExited)
                                                      DumpOutput(task.StandardOutput);

                                                  task.WaitForExit();

                                                  if (task.ExitCode != 0)
                                                      throw new Exception();
                                              });
        }

        private void DumpOutput(TextReader source)
        {
            for (var l = source.ReadLine(); l != null; l = source.ReadLine())
                Platform.Logger.WriteLine(l);
        }

        protected bool IsRunningOnMono
        {
            get { return Platform.IsMono; }
        }

        protected IDirectory RuntimeDirectory
        {
            get { return FileSystem.GetDirectory(RuntimeEnvironment.GetRuntimeDirectory()); }
        }

        public abstract Path Program { get; }
        protected abstract string GetArguments();
    }
}