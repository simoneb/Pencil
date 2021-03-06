﻿using System;
using System.Collections.Generic;
using Pencil.Tasks;

namespace Pencil.Test.Stubs
{
    public class StubOptions : IPencilOptions
    {
        public StubOptions(string buildScript, params string[] targets)
        {
            BuildScript = buildScript;
            Targets = targets;
        }

        public ICollection<string> Assemblies
        {
            get { throw new NotImplementedException(); }
        }

        public string BuildScript { get; set; }

        public IEnumerable<string> Targets { get; set; }

        public bool Help { get; set; }

        public bool NoLogo { get; set; }

        public CompilerVersion CompilerVersion { get; set; }

        public bool ShowTargets { get; set; }

        public void Display(Logger logger)
        {
        }
    }
}