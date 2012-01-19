using System;
using OpenFileSystem.IO;

namespace Pencil.Tasks
{
    public abstract class MSBuild3540Task : MSBuildTask
    {
        protected MSBuild3540Task()
        {
        }

        internal MSBuild3540Task(IFileSystem fileSystem, IExecutionEnvironment platform) : base(fileSystem, platform)
        {
        }

        public int? MaxCpuCount { get; set; }

        public MSBuildToolsVersion? ToolsVersion { get; set; }

        public bool? NodeReuse { get; set; }

        private string PickToolsVersion()
        {
            switch (ToolsVersion)
            {
                case MSBuildToolsVersion.v20:
                    return "2.0";
                case MSBuildToolsVersion.v30:
                    return "3.0";
                case MSBuildToolsVersion.v35:
                    return "3.5";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void AppendAdditionalArguments(CommandLineBuilder builder)
        {
            if (MaxCpuCount.HasValue)
                builder.Append("maxcpucount", MaxCpuCount);

            if (ToolsVersion.HasValue)
                builder.Append("toolsversion", PickToolsVersion());

            if (NodeReuse.HasValue)
                builder.Append("nodeReuse", NodeReuse);
        }
    }
}