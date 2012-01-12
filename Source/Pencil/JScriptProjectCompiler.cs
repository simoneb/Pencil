using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;

namespace Pencil
{
    internal class JScriptProjectCompiler : AbstractProjectCompiler
    {
        public JScriptProjectCompiler(ILogger logger, IEnumerable<string> referencedAssemblies) : base(logger, referencedAssemblies)
        {
        }

        public override IProject Compile(string scriptPath)
        {
            var assemblyPath = Path.GetTempFileName();
            var process = Process.Start(new ProcessStartInfo(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "jsc.exe"), BuildArguments(scriptPath, assemblyPath))
            {
                WindowStyle = ProcessWindowStyle.Hidden
            });

            process.WaitForExit();

            if(process.ExitCode != 0)
                throw new CompilationFailedException(new CompilerResults(new TempFileCollection()));

            return GetProject(Assembly.LoadFile(assemblyPath));
        }

        private string BuildArguments(string scriptPath, string assemblyPath)
        {
            return new StringBuilder()
                .AppendFormat("/r:{0} ", string.Join(";", ReferencedAssemblies.ToArray()))
                .AppendFormat("/out:{0} ", assemblyPath)
                .Append("/t:library ")
                .Append(Path.GetFullPath(scriptPath))
                .ToString();
        }
    }
}