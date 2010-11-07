using System.Collections.Generic;
using OpenFileSystem.IO.FileSystem.Local;

namespace Pencil
{
    public interface IPencilOptions
    {
        ICollection<Path> Assemblies { get; }
        string BuildScript { get; }
        IEnumerable<string> Targets { get; }
        bool Help { get; set; }
        bool NoLogo { get; }
        void Display(Logger logger);
    }
}