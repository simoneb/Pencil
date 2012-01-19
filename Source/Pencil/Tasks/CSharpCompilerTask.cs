using System.Linq;
using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;
using System;
using System.Text;

namespace Pencil.Tasks
{
    public enum OutputType
	{
		Library, Application, WindowsApplication, Module
	}

    public class CSharpCompilerTask : CompilerBaseTask
	{
	    public OutputType OutputType { get; set; }
		public bool Debug { get; set; }
		public bool Optimize { get; set; }

	    public CompilerVersion Version { get; set; }

        public CSharpCompilerTask()
        {
            Version = CompilerVersion.Default;
        }

        internal CSharpCompilerTask(IFileSystem fileSystem, IExecutionEnvironment platform): base(fileSystem, platform)
	    {
	        Version = CompilerVersion.Default;
	    }

	    public override Path Program
	    {
	        get
	        {
	            if (IsRunningOnMono)
	                return RuntimeDirectory.GetFile("gmcs.exe").Path;

	            return CompilerDirectory.Combine("csc.exe");
	        }
	    }

	    private Path CompilerDirectory
	    {
	        get
	        {
	            if (Version.Equals(CompilerVersion.V35))
	                return RuntimeDirectory.Parent.Path.Combine("v3.5");

                if (Version.Equals(CompilerVersion.V40))
                    return GuessCompilerDirectory("v4.0");

                throw new InvalidOperationException(string.Format("Path of compiler version {0} was not found", Version));
	        }
	    }

	    private Path GuessCompilerDirectory(string folderPrefix)
	    {
	        return RuntimeDirectory.Parent.Directories(folderPrefix + "*").FirstOrDefault().Path;
	    }

	    protected override string GetArguments()
		{
			if(Output == null)
				throw new InvalidOperationException("Output path is null.");

			References.CopyTo(FileSystem.GetFile(Output.FullPath).Parent.Path);

			return CollectArguments();
		}

        string CollectArguments()
        {
            var arguments = new StringBuilder("/nologo")
            	.AppendFormat(" /out:{0}", Output)
            	.AppendFormat(" /debug{0}", Debug ? "+" : "-")
            	.AppendFormat(" /target:{0}", GetTargetType());

            if(Optimize)
            	arguments.Append(" /optimize+");

            using(var r = References.GetEnumerator())
            {
                if(r.MoveNext())
                    arguments.AppendFormat(" /r:{0}", r.Current);
                while(r.MoveNext())
                    arguments.AppendFormat(",{0}", r.Current);
            }

            foreach(var x in Sources.Items)
    			arguments.AppendFormat(" {0}", x);

			return arguments.ToString();
        }

		string GetTargetType()
		{
			switch(OutputType)
			{
				case OutputType.Application: return "exe";
				case OutputType.WindowsApplication: return "winexe";
				case OutputType.Module: return "module";
				default: return "library";
			}
		}
	}
}