using NUnit.Framework;
using Pencil.Build.Tasks;

namespace Pencil.Test.Build.Tasks
{
    internal abstract class MSBuild3540TaskTestsBase<TTask> : MSBuildTaskTests<TTask> where TTask : MSBuild3540Task
    {
        [Test]
        public void Should_accept_max_cpu_count()
        {
            CheckArgument(t => t.MaxCpuCount = 2, " /maxcpucount:2");
        }

        [Test]
        public void Should_not_set_max_cpu_count_if_not_supplied()
        {
            CheckArgumentMissing("/maxcpucount");
        }

        [TestCase(MSBuildToolsVersion.v20, "2.0")]
        [TestCase(MSBuildToolsVersion.v30, "3.0")]
        [TestCase(MSBuildToolsVersion.v35, "3.5")]
        public void Should_accept_tools_version(MSBuildToolsVersion toolsVersion, string expectedArgumentValue)
        {
            CheckArgument(t => t.ToolsVersion = toolsVersion, " /toolsversion:" + expectedArgumentValue);
        }

        [Test]
        public void Should_not_set_toolsversion_if_not_supplied()
        {
            CheckArgumentMissing("/toolsversion");
        }

        [TestCase(true, "True")]
        [TestCase(false, "False")]
        public void Should_accept_node_reuse(bool reuse, string expectedArgumentValue)
        {
            CheckArgument(t => t.NodeReuse = reuse, " /nodeReuse:" + expectedArgumentValue);
        }

        [Test]
        public void Should_not_set_node_reuse_if_not_supplied()
        {
            CheckArgumentMissing("/nodeReuse");
        }
    }
}