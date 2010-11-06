using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;

namespace Pencil.Build.Tasks
{
    using System.Text;

    public class NUnitTask : ExecTaskBase
    {
        public NUnitTask(IFileSystem fileSystem, IExecutionEnvironment executionEnvironment) : base(fileSystem, executionEnvironment)
        {
            NUnitBinPath = new Path(".");
            ShowLogo = true;
            ShadowCopy = true;
        }

        public Path NUnitPath
        {
            get { return  NUnitBinPath.Combine("nunit-console.exe"); }
        }

        public Path NUnitBinPath { get; set; }
        public Path Target { get; set; }
        public bool ShadowCopy { get; set; }
        public bool ShowLogo { get; set; }

        public override Path Program
        {
            get { return IsRunningOnMono ? new Path("mono") : NUnitPath; }
        }

        protected override string GetArguments()
        {
            var args = new StringBuilder(IsRunningOnMono ? NUnitPath.ToString() : string.Empty);

            args.AppendFormat(" {0}", Target);

            if (!ShadowCopy)
                args.AppendFormat(" {0}", "-noshadow");

            if (!ShowLogo)
                args.AppendFormat(" {0}", "-nologo");

            return args.ToString();
        }
    }
}