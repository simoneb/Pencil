using System.Collections.Generic;
using Pencil.Tasks;

namespace Pencil
{
    internal class PencilOptions : IPencilOptions
    {
        private readonly PencilOptionsParser parser;

        public PencilOptions(PencilOptionsParser parser)
        {
            this.parser = parser;
            Assemblies = new HashSet<string>();
            Targets = new HashSet<string>();
            CompilerVersion = CompilerVersion.Default;
        }

        public ICollection<string> Assemblies { get; private set; }

        public string BuildScript { get; set; }

        public IEnumerable<string> Targets { get; set; }

        public bool Help { get; set; }

        public bool NoLogo { get; set; }

        public CompilerVersion CompilerVersion { get; set; }

        public bool ShowTargets { get; set; }

        public void Display(Logger logger)
        {
            parser.Display(logger);
        }
    }
}