using System.Linq;
using OpenFileSystem.IO;
using OpenFileSystem.IO.FileSystem.Local;

namespace Pencil.Build.Tasks
{
	using System;
    using System.Text;

    public enum OutputType
	{
		Library, Application, WindowsApplication, Module
	}

    public enum CompilerVersion
    {
        v35, v40
    }

	public class CSharpCompilerTask : CompilerBaseTask
	{
	    public OutputType OutputType { get; set; }
		public bool Debug { get; set; }
		public bool Optimize { get; set; }

	    public CompilerVersion Version { get; set; }

	    public CSharpCompilerTask(IFileSystem fileSystem, IExecutionEnvironment platform): base(fileSystem, platform)
	    {
	    }

	    protected override Path GetProgramCore()
		{
			if(IsRunningOnMono)
				return RuntimeDirectory.GetFile("gmcs.exe").Path;

			return CompilerDirectory.Combine("csc.exe");
		}

	    private Path CompilerDirectory
	    {
	        get
	        {
	            switch (Version)
	            {
	                case CompilerVersion.v35:
	                    return RuntimeDirectory.Parent.Path.Combine("v3.5");
	                case CompilerVersion.v40:
	                    return GuessCompilerDirectory("v4.0");
	                default:
                        throw new InvalidOperationException(string.Format("Path of compiler version {0} was not found", Version));
                }
	        }
	    }

	    private Path GuessCompilerDirectory(string folderPrefix)
	    {
	        return RuntimeDirectory.Parent.Directories(folderPrefix + "*").FirstOrDefault().Path;
	    }

	    protected override string GetArgumentsCore()
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
