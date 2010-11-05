using System.Collections.Generic;
using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;
using System.Linq;

namespace Pencil.Build.Tasks
{
    public abstract class MSBuildTask : ExecTaskBase
    {
        protected MSBuildTask(IFileSystem fileSystem, IExecutionEnvironment platform) : base(fileSystem, platform)
        {
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

        protected IDirectory FrameworksDirectory
        {
            get { return RuntimeDirectory.Parent; }
        }

        protected override sealed Path GetProgramCore()
        {
            return GetMSBuildPath();
        }

        protected abstract Path GetMSBuildPath();

        protected override string GetArgumentsCore()
        {
            var builder = new CommandLineBuilder();

            if (Targets.Any())
                builder.Append("target", string.Join(";", Targets));

            foreach (var property in Properties)
                builder.Append("property", property.Key + "=" + property.Value);

            if(setVerbosity)
                builder.Append("verbosity", Verbosity.ToString().ToLowerInvariant());

            builder.Append(ProjectFile ?? GetFirstSolutionInCurrentDir());

            AppendAdditionalArguments(builder);

            return builder.ToString();
        }

        private string GetFirstSolutionInCurrentDir()
        {
            return FileSystem.GetDirectory(".").Files("*.sln").Select(file => file.ToString()).FirstOrDefault();
        }

        protected abstract void AppendAdditionalArguments(CommandLineBuilder builder);
    }
}