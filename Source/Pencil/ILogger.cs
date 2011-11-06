using System;

namespace Pencil
{
    public interface ILogger
    {
        void WriteLine(string format, params object[] args);
        void Write(string format, params object[] args);
        void WriteLine();
        IDisposable Indent();
    }
}