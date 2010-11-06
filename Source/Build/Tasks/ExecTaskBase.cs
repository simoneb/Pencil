using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;

namespace Pencil.Build.Tasks
{
    using System;
    using System.Runtime.InteropServices;
    using IO;

    public abstract class ExecTaskBase
    {
        protected IFileSystem FileSystem { get; private set; }
        protected IExecutionEnvironment Platform { get; private set; }

        public Path Program
        {
            get { return GetProgramCore(); }
        }

        protected ExecTaskBase(IFileSystem fileSystem, IExecutionEnvironment platform)
        {
            FileSystem = fileSystem;
            Platform = platform;
        }

        public bool ShowCommandLine { get; set; }

        public void Execute()
        {
            var fileName = Program.ToString();
            var arguments = GetArgumentsCore();

            if(ShowCommandLine)
            {
                Platform.StandardOut.WriteLine("Starting {0} {1}", fileName, arguments);
                Platform.StandardOut.WriteLine("in directory {0}", Platform.CurrentDirectory);
            }

            Platform.Run(fileName, arguments, task =>
                                              {
                                                  while (!task.HasExited)
                                                      task.StandardOutput.CopyTo(Platform.StandardOut);

                                                  task.WaitForExit();

                                                  if (task.ExitCode != 0)
                                                      throw new Exception();
                                              });
        }

        protected bool IsRunningOnMono
        {
            get { return Platform.IsMono; }
        }

        protected IDirectory RuntimeDirectory
        {
            get { return FileSystem.GetDirectory(RuntimeEnvironment.GetRuntimeDirectory()); }
        }

        protected abstract Path GetProgramCore();
        protected abstract string GetArgumentsCore();
    }
}