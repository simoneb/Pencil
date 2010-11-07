using System.Collections.Generic;
using OpenFileSystem.IO.FileSystem.Local;
using Pencil.Tasks;

namespace Pencil
{
    internal class PencilOptions : IPencilOptions
    {
        private readonly PencilOptionsParser parser;

        public PencilOptions(PencilOptionsParser parser)
        {
            this.parser = parser;
            Assemblies = new HashSet<Path>();
            Targets = new HashSet<string>();
            CompilerVersion = CompilerVersion.Default;
        }

        public ICollection<Path> Assemblies { get; private set; }

        public string BuildScript { get; set; }

        public IEnumerable<string> Targets { get; set; }

        public bool Help { get; set; }

        public bool NoLogo { get; set; }

        public CompilerVersion CompilerVersion { get; set; }

        public void Display(Logger logger)
        {
            parser.Display(logger);
        }
    }
}