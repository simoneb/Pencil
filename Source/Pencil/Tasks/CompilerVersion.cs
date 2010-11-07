using System;
using System.Collections.Generic;
using System.Linq;

namespace Pencil.Tasks
{
    public class CompilerVersion : IEquatable<CompilerVersion>
    {
        public string CodePoviderName { get; set; }

        public static IEnumerable<CompilerVersion> All
        {
            get { return new[] {V35, V40}; }
        }

        public static CompilerVersion Default { get { return V35; } }

        public static readonly CompilerVersion  V35 = new CompilerVersion("v3.5");
        public static readonly CompilerVersion  V40 = new CompilerVersion("v4.0");

        private CompilerVersion(string codePoviderName)
        {
            CodePoviderName = codePoviderName;
        }

        public static CompilerVersion FromName(string name)
        {
            return All.Single(version => version.CodePoviderName.Equals(name));
        }

        public bool Equals(CompilerVersion other)
        {
            return ReferenceEquals(this, other);
        }
    }
}