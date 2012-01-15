using System.Collections.Generic;
using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;
using System.Linq;

namespace Pencil.Tasks
{
    public abstract class MSBuildTask : ExecTaskBase
    {
        protected MSBuildTask(IFileSystem fileSystem, IExecutionEnvironment platform) : base(fileSystem, platform)
        {
            Targets = new List<string>();
            Properties = new Dictionary<string, string>();
        }

        public string ProjectFile { get; set; }
        public IList<string> Targets { get; set; }
        public IDictionary<string, string> Properties { get; private set; }

        public void AddProperty(string name, string value)
        {
            Properties.Add(name, value);
        }

        public MSBuildVerbosity? Verbosity { get; set; }

        private IDirectory FrameworksDirectory
        {
            get { return RuntimeDirectory.Parent; }
        }

        public override Path Program
        {
            get { return MSBuildPath; }
        }

        private Path MSBuildPath
        {
            get
            {
                return FrameworksDirectory.Directories(FrameworkDirectorySearchPattern).First().GetFile("msbuild.exe").Path;
            }
        }
        protected abstract string FrameworkDirectorySearchPattern { get; }

        protected override string GetArguments()
        {
            var builder = new CommandLineBuilder();

            if (Targets.Any())
                builder.Append("target", string.Join(";", Targets.ToArray()));

            foreach (var property in Properties)
                builder.Append("property", string.Format("{0}={1}", property.Key, property.Value));

            if(Verbosity.HasValue)
                builder.Append("verbosity", Verbosity.ToString().ToLowerInvariant());

            builder.Append(ProjectFile ?? GetFirstSolutionInCurrentDir());

            AppendAdditionalArguments(builder);

            return builder.ToString();
        }

        private string GetFirstSolutionInCurrentDir()
        {
            return FileSystem.GetDirectory(".").Files("*.sln").Select(file => file.ToString()).FirstOrDefault();
        }

        protected virtual void AppendAdditionalArguments(CommandLineBuilder builder){}
    }
}