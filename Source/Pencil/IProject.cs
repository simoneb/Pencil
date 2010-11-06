using OpenFileSystem.IO;

namespace Pencil
{
    public interface IProject
    {
        bool HasTarget(string name);
        void Run(string target);
		void Register<T>(T instance);
        bool HasDefaultTarget { get; }
        string DefaultTarget { get; }
        IFileSystem FileSystem { get; }
        IExecutionEnvironment Platform { get; }
    }
}