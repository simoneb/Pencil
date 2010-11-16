using System.IO;

namespace Pencil
{
    public interface IProcess
    {
        bool HasExited { get; }
        int ExitCode { get; }
        TextReader StandardOutput { get; }
        void WaitForExit();
    }
}