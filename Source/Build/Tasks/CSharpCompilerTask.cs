using System.Linq;

namespace Pencil.Build.Tasks
{
	using System;
    using System.Text;
    using IO;

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
	    private readonly IFileSystem fileSystem;
	    public OutputType OutputType { get; set; }
		public bool Debug { get; set; }
		public bool Optimize { get; set; }

	    public CompilerVersion Version { get; set; }

	    public CSharpCompilerTask(IFileSystem fileSystem, 
            IExecutionEnvironment executionEnvironment): base(fileSystem, executionEnvironment)
	    {
	        this.fileSystem = fileSystem;
	    }

	    protected override Path GetProgramCore()
		{
			if(IsRunningOnMono)
				return RuntimeDirectory + "gmcs.exe";

			return CompilerDirectory + "csc.exe";
		}

	    private Path CompilerDirectory
	    {
	        get
	        {
	            switch (Version)
	            {
	                case CompilerVersion.v35:
	                    return RuntimeDirectory.Parent + "v3.5";
	                case CompilerVersion.v40:
	                    return GuessCompilerDirectory("v4.0");
	                default:
                        throw new InvalidOperationException(string.Format("Path of compiler version {0} was not found", Version));
                }
	        }
	    }

	    private Path GuessCompilerDirectory(string folderPrefix)
	    {
	        return fileSystem.GetDirectories(RuntimeDirectory.Parent, folderPrefix + "*").FirstOrDefault();
	    }

	    protected override string GetArgumentsCore()
		{
			if(Output == null)
				throw new InvalidOperationException("Output path is null.");
			References.CopyTo(Output.GetDirectory());
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
