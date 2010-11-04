namespace Pencil.Build
{
    using System;
    using System.IO;
    using System.Diagnostics;

    internal sealed class ExecutionEnvironment : IExecutionEnvironment
    {
        private readonly TextWriter standardOut;

        public ExecutionEnvironment(TextWriter standardOut)
        {
            this.standardOut = standardOut;
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
        }

        public TextWriter StandardOut
        {
            get { return standardOut; }
        }

        public bool IsMono
        {
            get { return Type.GetType("Mono.Runtime") != null; }
        }
    }
}