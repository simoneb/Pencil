namespace Pencil
{
    public interface IProject
    {
        bool HasTarget(string name);
        void Run(string target);
		void Register<T>(T instance);
        bool HasDefaultTarget { get; }
        string DefaultTarget { get; }
    }
}