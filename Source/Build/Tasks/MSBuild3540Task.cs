using System;
using System.Text;
using Pencil.IO;

namespace Pencil.Build.Tasks
{
    public abstract class MSBuild3540Task : MSBuildTask
    {
        private int maxCpuCount;
        private bool setMaxCpuCount;
        private MSBuildToolsVersion toolsVersion;
        private bool setToolsVersion;
        private bool nodeReuse;
        private bool setNodeReuse;

        protected MSBuild3540Task(IFileSystem fileSystem, IExecutionEnvironment platform) : base(fileSystem, platform)
        {
        }

        public int MaxCpuCount
        {
            get { return maxCpuCount; }
            set
            {
                setMaxCpuCount = true;
                maxCpuCount = value;
            }
        }

        public MSBuildToolsVersion ToolsVersion
        {
            get {
                return toolsVersion;
            }
            set
            {
                setToolsVersion = true;
                toolsVersion = value;
            }
        }

        private string PickToolsVersion
        {
            get {
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
        }

        public bool NodeReuse
        {
            get {
                return nodeReuse;
            }
            set
            {
                setNodeReuse = true;
                nodeReuse = value;
            }
        }

        protected override void AppendAdditionalArguments(StringBuilder builder)
        {
            if (setMaxCpuCount)
                builder.Append(" /maxcpucount:").Append(MaxCpuCount);

            if(setToolsVersion)
                builder.Append(" /toolsversion:").Append((string) PickToolsVersion);

            if (setNodeReuse)
                builder.Append(" /nodeReuse:").Append((bool) NodeReuse);
        }
    }
}