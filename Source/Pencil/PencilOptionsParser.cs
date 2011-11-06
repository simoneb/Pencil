using System.Collections.Generic;
using Mono.Options;
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
                { 
                    "r|reference=", 
                    "References the specified {ASSEMBLY} when compiling the build script", 
                    x => result.Assemblies.Add(x) 
                },
                { 
                    "c|compiler=", 
                    string.Format("The version of the C# compiler to use to compile the build script. Available values: {0}. Default is {1}", Compilers, DefaultCompiler), 
                    c => result.CompilerVersion = CompilerVersion.FromName(c) },
                { "t|targets", "Display a list of available targets and exits", x => result.ShowTargets = true },
                { "nologo", "Do not display the application logo", x => result.NoLogo = true },
                { "h|help|?", "Display this help and exits", ignored => result.Help = true }
            };
        }

        private static string DefaultCompiler
        {
            get { return CompilerVersion.Default.CodePoviderName; }
        }

        private static string Compilers
        {
            get { return CompilerVersion.All.Select(c => c.CodePoviderName).Join(", "); }
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
            logger.WriteLine("Usage: Pencil.exe [options] <path to build script> [targets]");
            logger.WriteLine();
            inner.WriteOptionDescriptions(logger.Target);
            logger.WriteLine();
        }
    }
}