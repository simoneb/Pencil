namespace Pencil
{
    public class FutureProject : Future<IProject>
    {
        private readonly IProject parent;
        private readonly string file;

        public FutureProject(IProject parent, string file)
        {
            this.parent = parent;
            this.file = file;
        }

        protected override IProject Evaluate()
        {
            var compiler = GetCompiler();
            return compiler.Compile(file);
        }

        private IProjectCompiler GetCompiler()
        {
            return new CSharpProjectCompiler(parent.Logger, parent.ReferencedAssemblies, CompilerVersion.Default);
        }
    }
}