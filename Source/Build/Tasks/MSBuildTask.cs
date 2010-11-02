using System.Collections.Generic;
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
            Properties = new List<KeyValuePair<string, string>>();
        }

        public string ProjectFile { get; set; }
        public string[] Targets { get; set; }
        private IList<KeyValuePair<string, string>> Properties { get; set; }
        private MSBuildVerbosity verbosity;
        private bool setVerbosity;

        public void AddProperty(string name, string value)
        {
            Properties.Add(new KeyValuePair<string, string>(name, value));
        }

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

            if (Properties.Any())
                builder.Append(" /property:")
                    .Append(string.Join(";", Properties.Select(e => e.Key + "=" + e.Value).ToArray()));

            if(setVerbosity)
                builder.Append(" /verbosity:").Append(Verbosity.ToString().ToLowerInvariant());

            builder.Append(" ").Append(ProjectFile ?? GetFirstSolutionInCurrentDir());

            AppendAdditionalArguments(builder);

            return builder.ToString();
        }

        private string GetFirstSolutionInCurrentDir()
        {
            return FileSystem.GetFiles(Path.Empty, "*.sln").Select(path => path.ToString()).
                FirstOrDefault();
        }

        protected abstract void AppendAdditionalArguments(StringBuilder builder);
    }
}