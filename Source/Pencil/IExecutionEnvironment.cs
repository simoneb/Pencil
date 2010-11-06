using System;

namespace Pencil
{
    public interface IExecutionEnvironment
    {
        void Run(string fileName, string arguments, Action<IProcess> processHandler);
		Logger Logger { get; }
		bool IsMono { get; }
        string CurrentDirectory { get; }
    }
}
