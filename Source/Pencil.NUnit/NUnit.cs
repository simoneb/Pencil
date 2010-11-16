using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        
    }
}
