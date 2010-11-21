namespace Pencil
{
    public class FutureProject : Future<IProject>
    {
        private readonly Project project;
        private readonly string file;

        public FutureProject(Project project, string file)
        {
            this.project = project;
            this.file = file;
        }

        protected override IProject Evaluate()
        {
            var compiler = GetCompiler();
            return compiler.Compile(file);
        }

        private IProjectCompiler GetCompiler()
        {
            return new CSharpProjectCompiler(project.Logger, project.ReferencedAssemblies, CompilerVersion.Default);
        }
    }
}