using System;
using System.IO;

namespace Pencil
{
    public class Logger
	{
		public static readonly Logger Null = new Logger(TextWriter.Null);

	    string indentation = string.Empty;

		public Logger(TextWriter target)
		{
			Target = target;
		}

	    public TextWriter Target { get; private set; }

	    public void Write(string format, params object[] args)
		{
			Target.Write(indentation);
			Target.WriteLine(format, args);
		}

		public IDisposable Indent()
		{
			string old = indentation;
			indentation = old + "    ";
			return new Finally(() => indentation = old);
		}

		sealed class Finally : IDisposable
		{
		    readonly Action action;

			public Finally(Action action)
			{
				this.action = action;
			}

			public void Dispose(){ action(); }
		}
	}
}