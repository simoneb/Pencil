using System.Collections;
using System.Text;
using Pencil.IO;
using System.Linq;

namespace Pencil.Build.Tasks
{
    public abstract class MSBuildTask : ExecTaskBase
    {
        protected IFileSystem FileSystem { get; private set; }

        protected MSBuildTask(IFileSystem fileSystem, IExecutionEnvironment platform) : base(platform)
        {
            FileSystem = fileSystem;
            Targets = new string[0];
            Properties = new Hashtable();
        }

        public string ProjectFile { get; set; }
        public string[] Targets { get; set; }
        public Hashtable Properties { get; set; }
        private MSBuildVerbosity verbosity;
        private bool setVerbosity;

        public MSBuildVerbosity Verbosity
        {
            get { return verbosity; }
            set
            {
                setVerbosity = true;
                verbosity = value;
            }
        }

        protected Path FrameworksDirectory
        {
            get { return RuntimeDirectory + ".."; }
        }

        protected override sealed Path GetProgramCore()
        {
            return GetMSBuildPath();
        }

        protected abstract Path GetMSBuildPath();

        protected override string GetArgumentsCore()
        {
            var builder = new StringBuilder();

            if (Targets.Any())
                builder.Append(" /target:")
                    .Append(string.Join(";", Targets));

            if (Properties.Count != 0)
                builder.Append(" /property:")
                    .Append(string.Join(";",
                                        Properties.Cast<DictionaryEntry>().Select(e => e.Key + "=" + e.Value).ToArray()));

            if(setVerbosity)
                builder.Append(" /verbosity:").Append(Verbosity.ToString().ToLowerInvariant());

            builder.Append(" ").Append(ProjectFile ??
                                       FileSystem.GetFiles(Path.Empty, "*.sln").Select(path => path.ToString()).
                                           FirstOrDefault());

            AppendAdditionalArguments(builder);

            return builder.ToString();
        }

        protected abstract void AppendAdditionalArguments(StringBuilder builder);
    }
}