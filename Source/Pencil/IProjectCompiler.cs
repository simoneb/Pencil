namespace Pencil
{
    public interface IProjectCompiler
    {
        IProject Compile(string path);
    }
}