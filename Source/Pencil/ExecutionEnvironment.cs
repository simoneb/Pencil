using System;
using System.Diagnostics;

namespace Pencil
{
    internal sealed class ExecutionEnvironment : IExecutionEnvironment
    {
        private readonly Logger logger;

        public ExecutionEnvironment(Logger logger)
        {
            this.logger = logger;
        }

        public void Run(string fileName, string arguments, Action<IProcess> processHandler)
        {
            var startInfo = new ProcessStartInfo
                            {
                                FileName = fileName,
                                Arguments = arguments,
                                UseShellExecute = false,
                                RedirectStandardOutput = true
                            };

            using (var process = Process.Start(startInfo))
                processHandler(new ProcessAdapter(process));
        }

        public string CurrentDirectory
        {
            get { return Environment.CurrentDirectory; }
            set { Environment.CurrentDirectory = value; }
        }

        public Logger Logger
        {
            get { return logger; }
        }

        public bool IsMono
        {
            get { return Type.GetType("Mono.Runtime") != null; }
        }
    }
}