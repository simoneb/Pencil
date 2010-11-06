namespace Pencil.Test.Stubs
{
	using System;

    public class ExecutionEnvironmentStub : IExecutionEnvironment
    {
        public Action<string, string, Action<IProcess>> RunHandler = (x, y, z) => {};
		public Func<bool> IsMonoHandler = () => false;
		public Func<string> CurrentDirectoryHandler = () => "";

        public void Run(string fileName, string arguments, Action<IProcess> processHandler)
        {
            RunHandler(fileName, arguments, processHandler);
        }

		public Logger Logger { get { return new Logger(Console.Out); } }
		public bool IsMono { get { return IsMonoHandler(); } }

        public string CurrentDirectory
        {
            get { return CurrentDirectoryHandler(); }
        }
    }
}
