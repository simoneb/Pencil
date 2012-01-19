using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenFileSystem.IO.FileSystems;
using Pencil.Attributes;

namespace Pencil.NUnit
{
    class NUnit : Project
    {
        public string ProjectName { get { return "NUnit"; } }
        public string BasePackageVersion { get { return "2.5.9"; } }
        public string NominalVersion { get { return "2.5.9"; } }
        public DateTime TempNow { get { return DateTime.Now; } }
        public int TempYear { get { return TempNow.Year - 2000; } }
        public string TempDay { get { return TempNow.DayOfYear.ToString().PadLeft(3, '0'); } }
        public string PackageBuildNumber { get { return string.Format("{0}{1}", TempYear, TempDay); } }
        public string PackageVersion { get { return string.Format("{0}.{1}", BasePackageVersion, PackageBuildNumber); } }

        public string PackageConfiguration { get { return string.Empty; } }
        public string PackageName { get { return string.Format("{0}-{1}", ProjectName, PackageVersion); } }

        public IEnumerable<string> SupportedFrameworks { get { return new[] { "net-2.0", "net-3.5", "net-4.0", "net-1.1", "net-1.0", "mono-2.0", "mono-1.0" }; } }

        public string StandardPackages { get { return "std"; } }
        public string DefaultPackageConfig { get { return "std"; } }

        public string NUnitOptions { get; set; }

        public NUnit()
        {
            //Include("NUnit.Include.cs");
        }

        [Description("Displays additional help information")]
        [Default]
        public void Help()
        {
            Console.WriteLine(@"This build file will build NUnitLite for any of the supported
    runtime frameworks which are actually installed. To add or
    support for a framework, edit this script

    Running on the current system, the following runtime frameworks
    are available for building and testing NUnit:");

            //foreach (var framework in Property<IEnumerable<string>>("InstalledFrameworks"))
            //    Console.WriteLine(framework.PadRight(15, ' ') + GetFrameworkDescription(framework));

            var defaultNetTarget = "Not Available";
            var defaultMonoTarget = "Not Available";

            //if (!string.IsNullOrEmpty(Property<string>("DefaultNetRuntime")))
            //    defaultNetTarget = Property<string>("DefaultNetRuntime");

            //if (!string.IsNullOrEmpty(Property<string>("DefaultMonoRuntime")))
            //    defaultMonoTarget = Property<string>("DefaultMonoRuntime");

            Console.WriteLine(@"The default build target is the {0} debug config.
    Generic runtime targets use the following defaults:
        net           {1}
        mono          {2}

    Note that targets that set the build configuration or runtime
    to be used must come before action targets. For example:

        nant net-1.1 release build
        nant build-all
        nant debug clean build

    Use   nant -projecthelp to see a full list of targets.", /*Property<string>("DefaultRuntime")*/ "", defaultNetTarget, defaultMonoTarget);
        }

        private string GetFrameworkDescription(string framework)
        {
            return framework;
        }

        [DependsOn("SetDebugBuildConfig")]
        [Description("Set config to debug for commands that follow")]
        public void Debug()
        {
        }

        [DependsOn("SetReleaseBuildConfig")]
        [Description("Set config to release for commands that follow")]
        public void Release()
        {
        }

        [DependsOn("SetDefaultDotNetRuntimeConfig")]
        [Description("Set runtime to .NET 1.1 for commands that follow")]
        public void Net()
        {}

        [DependsOn("SetNet10RuntimeConfig")]
        [Description("Set runtime to .NET 1.0 for commands that follow")]
        public void Net10()
        { }

        [DependsOn("SetNet11RuntimeConfig")]
        [Description("Set runtime to .NET 1.1 for commands that follow")]
        public void Net11()
        { }

        [DependsOn("SetNet20RuntimeConfig")]
        [Description("Set runtime to .NET 2.0 for commands that follow")]
        public void Net20()
        { }

        [DependsOn("SetNet35RuntimeConfig")]
        [Description("Set runtime to .NET 3.5 for commands that follow")]
        public void Net35()
        { }

        [DependsOn("SetNet40RuntimeConfig")]
        [Description("Set runtime to .NET 4.0 for commands that follow")]
        public void Net40()
        { }

        [DependsOn("SetDefaultMonoRuntimeConfig")]
        [Description("Set runtime to Mono 1.0 for commands that follow")]
        public void Mono()
        { }

        [DependsOn("SetMono10RuntimeConfig")]
        [Description("Set runtime to Mono 1.0 for commands that follow")]
        public void Mono10()
        { }

        [DependsOn("SetMono20RuntimeConfig")]
        [Description("Set runtime to Mono 2.0 for commands that follow")]
        public void Mono20()
        { }

        [DependsOn("SetBuildDir")]
        [Description("Removes output created by the current build config")]
        public void Clean()
        {
            var currentBuildDir = FileSystem.GetDirectory(/*Property<string>("CurrentBuildDir")*/ "");

            if(currentBuildDir.Exists)
                currentBuildDir.Delete();

            var generatedAssemblyInfo = FileSystem.GetFile("src/GeneratedAssemblyInfo.cs");

            if(generatedAssemblyInfo.Exists)
                generatedAssemblyInfo.Delete();
        }

        [Description("Removes output created by all build configs")]
        public void CleanAll()
        {
            var projectBuildDir = FileSystem.GetDirectory("" /*"Property<string>("ProjectBuildDir")*/);

            if (projectBuildDir.Exists)
                projectBuildDir.Delete();

            var generatedAssemblyInfo = FileSystem.GetFile("src/GeneratedAssemblyInfo.cs");

            if (generatedAssemblyInfo.Exists)
                generatedAssemblyInfo.Delete();
        }

        public void CleanPackageDir()
        {
            var packageWorkingDir = FileSystem.GetDirectory("" /*"Property<string>("PackageWorkingDir")*/);

            if (packageWorkingDir.Exists)
                packageWorkingDir.Delete();
        }

        public void GenAssemblyInfo()
        {
            // TODO
    //        <asminfo output="src/GeneratedAssemblyInfo.cs" language="CSharp">
    //  <imports>
    //    <import namespace="System.Reflection"/>
    //  </imports>
    //  <attributes>
    //    <attribute type="AssemblyCompanyAttribute" value="NUnit.org"/>
    //    <attribute type="AssemblyProductAttribute" value="NUnit"/>
    //    <attribute type="AssemblyCopyrightAttribute"
    //      value="Copyright (C) 2002-2009 Charlie Poole.&#xD;&#xA;Copyright (C) 2002-2004 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov.&#xD;&#xA;Copyright (C) 2000-2002 Philip Craig.&#xD;&#xA;All Rights Reserved."/>
    //    <attribute type="AssemblyTrademarkAttribute" value="NUnit is a trademark of NUnit.org"/>
    //    <attribute type="AssemblyVersionAttribute" value="${package.version}"/>
    //    <attribute type="AssemblyInformationalVersionAttribute" value="${package.version}"/>
    //    <attribute type="AssemblyConfigurationAttribute" value="${package.configuration}"/>
    //  </attributes>
    //</asminfo>
        }

        [Description("Generate code for the fluent constraint builder interface")]
        public void GenSyntax()
        {
            Process.Start(new ProcessStartInfo(/*Property<string>("ProjectToolsDir") +*/ "/bin/GenSyntax.exe", "SyntaxElements.txt")
                          {
                              WorkingDirectory = /*Property<string>("ProjectSrcDir") +*/ "/NUnitFramework/framework"
                          });
        }

        [DependsOn("MakeBuildDir")]
        [DependsOn("GenAssemblyInfo")]
        [Description("Build NUnit for default runtime version and config")]
        public void Build()
        {
            //Console.WriteLine("*");
            //Console.WriteLine("Starting {0} {1} build", Property<string>("RuntimeConfig"), Property<string>("BuildConfig"));
            //Console.WriteLine("*");

            //FileSystem.GetFile(Property<string>("ProjectBaseDir") + "/nunit.snk")
            //    .CopyTo(FileSystem.GetDirectory(Property<string>("ProjectBuildDir")));

            //foreach (var f in FileSystem.GetDirectory(Property<string>("ProjectLibDir")).Files("*.dll", SearchScope.CurrentOnly))
            //    f.CopyTo(FileSystem.GetDirectory(Property<string>("CurrentLibDir")));

            //foreach (var f in Property<string[]>("ProjectBuildFiles"))
            //    Pencil(f, "Build");

            //if(Property<bool>("BuildGui"))
            //    Call(BuildGui);


        }

        public void BuildGui()
        {
            //if(!Property<bool>("BuildGui"))
            //    throw new InvalidOperationException("Runtime 2.0 or greater is required to build the NUnit GUI");

            //Pencil(Property<string>("GuiBuildFiles"), "Build");
        }
    }
}
