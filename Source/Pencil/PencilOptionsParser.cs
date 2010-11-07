using System.Collections.Generic;
using Mono.Options;
using OpenFileSystem.IO.FileSystem.Local;
using System.Linq;

namespace Pencil
{
    public class PencilOptionsParser
    {
        private readonly OptionSet inner;
        private readonly PencilOptions result;

        public PencilOptionsParser()
        {
            result = new PencilOptions(this);

            inner = new OptionSet
            {
                { "r|reference=", "References the specified {ASSEMBLY} when compiling the build script", x => result.Assemblies.Add(new Path(x)) },
                { "nologo", "Do not display the application logo", x => result.NoLogo = true },
                { "h|help|?", "Display this help", ignored => result.Help = true }
            };
        }

        public IPencilOptions Parse(params string[] args)
        {
            var unparsed = inner.Parse(args);

            result.BuildScript = unparsed.FirstOrDefault();
            result.Targets = new HashSet<string>(unparsed.Skip(1));

            return result;
        }

        public void Display(Logger logger)
        {
            logger.Write("Usage: Pencil.exe [options] {{path to build script}} [targets]");
            logger.WriteLine();
            inner.WriteOptionDescriptions(logger.Target);
            logger.WriteLine();
        }
    }
}