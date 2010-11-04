namespace Pencil.Build.Tasks
{
    using System;
    using System.Runtime.InteropServices;
    using IO;

    public abstract class ExecTaskBase
    {
        private readonly IExecutionEnvironment platform;

        public Path Program
        {
            get { return GetProgramCore(); }
        }

        protected ExecTaskBase(IExecutionEnvironment platform)
        {
            this.platform = platform;
        }

        public bool ShowCommandLine { get; set; }

        public void Execute()
        {
            var fileName = Program.ToString();
            var arguments = GetArgumentsCore();

            if(ShowCommandLine)
                platform.StandardOut.WriteLine("Running {0} {1} in folder {2}", fileName, arguments, platform.CurrentDirectory);

            platform.Run(fileName, arguments, task =>
                                              {
                                                  while (!task.HasExited)
                                                      task.StandardOutput.CopyTo(platform.StandardOut);

                                                  task.WaitForExit();

                                                  if (task.ExitCode != 0)
                                                      throw new Exception();
                                              });
        }

        protected bool IsRunningOnMono
        {
            get { return platform.IsMono; }
        }

        protected Path RuntimeDirectory
        {
            get { return new Path(RuntimeEnvironment.GetRuntimeDirectory()); }
        }

        protected abstract Path GetProgramCore();
        protected abstract string GetArgumentsCore();
    }
}