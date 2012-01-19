using System.Collections;
using System.Collections.Generic;
using Pencil.Tasks;

namespace Pencil
{
    public partial class Project
    {
        public void MSBuild4(string projectFile,
                             IList<string> targets,
                             Hashtable properties = null,
                             int? maxCpuCount = null,
                             bool? nodeReuse = null,
                             bool showCommandLine = false,
                             MSBuildToolsVersion? toolsVersion = null,
                             MSBuildVerbosity? verbosity = null)
        {
             var task = new MSBuild40Task(FileSystem, Platform)
             {
                 ProjectFile = projectFile,
                 Targets = targets,
                 MaxCpuCount = maxCpuCount,
                 NodeReuse = nodeReuse,
                 ShowCommandLine = showCommandLine,
                 ToolsVersion = toolsVersion,
                 Verbosity = verbosity
             };

             foreach (var property in properties ?? new Dictionary<string, string>())
                 task.AddProperty(property.Key, property.Value);

             task.Run();
         }
    }
}