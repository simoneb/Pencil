using System.Collections.Generic;
using OpenFileSystem.IO.FileSystem.Local;
using Pencil.Tasks;

namespace Pencil
{
    public interface IPencilOptions
    {
        ICollection<string> Assemblies { get; }
        string BuildScript { get; }
        IEnumerable<string> Targets { get; }
        bool Help { get; set; }
        bool NoLogo { get; }
        CompilerVersion CompilerVersion { get; }
        bool ShowTargets { get; set; }
        void Display(Logger logger);
    }
}