using System;
using System.IO;

namespace Pencil
{
    public class Logger : ILogger
    {
		public static readonly Logger Null = new Logger(TextWriter.Null);

	    string indentation = string.Empty;

		public Logger(TextWriter target)
		{
			Target = target;
		}

	    public TextWriter Target { get; private set; }

	    public void WriteLine(string format, params object[] args)
		{
			Target.Write(indentation);
			Target.WriteLine(format, args);
		}

        public void Write(string format, params object[] args)
        {
            Target.Write(indentation);
            Target.Write(format, args);
        }

        public void WriteLine()
        {
            Target.WriteLine();
        }

		public IDisposable Indent()
		{
			string old = indentation;
			indentation = old + "   ";
			return new DisposableAction(() => indentation = old);
		}
	}
}