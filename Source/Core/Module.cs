using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pencil.Core
{
    class PencilModule : IModule
    {
        readonly ITypeLoader typeLoader;
        readonly Module module;

        public PencilModule(ITypeLoader typeLoader, Module module){
            this.typeLoader = typeLoader;
            this.module = module;
        }

        public string Name { get { return module.Name; } }

        public IEnumerable<IType> Types
        {
            get { return module.GetTypes().Select(typeLoader.FromNative); }
        }
    }
}
