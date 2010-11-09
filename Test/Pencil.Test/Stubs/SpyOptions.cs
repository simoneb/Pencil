using System.Collections.Generic;
using Pencil.Tasks;

namespace Pencil.Test.Stubs
{
    public class SpyOptions : IPencilOptions
    {
        public bool Displayed;

        public ICollection<string> Assemblies { get; set; }

        public string BuildScript { get; set; }

        public IEnumerable<string> Targets { get; set; }

        public bool Help
        {
            get; set;
        }

        public bool NoLogo
        {
            get; set;
        }

        public CompilerVersion CompilerVersion
        {
            get; set;
        }

        public bool ShowTargets
        {
            get; set;
        }

        public void Display(Logger logger)
        {
            Displayed = true;
        }
    }
}