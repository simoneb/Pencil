using System.Diagnostics;

namespace Pencil
{
    internal sealed class ProcessAdapter : IProcess
    {
        private readonly Process process;

        public ProcessAdapter(Process process)
        {
            this.process = process;
        }

        public bool HasExited
        {
            get { return process.HasExited; }
        }

        public int ExitCode
        {
            get { return process.ExitCode; }
        }

        public System.IO.TextReader StandardOutput
        {
            get { return process.StandardOutput; }
        }

        public void WaitForExit()
        {
            process.WaitForExit();
        }
    }
}