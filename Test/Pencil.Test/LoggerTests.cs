using System.IO;
using NUnit.Framework;

namespace Pencil.Test
{
    [TestFixture]
	public class LoggerTests
	{
		[Test]
		public void Should_support_using_for_Indent()
		{
			var output = new StringWriter();
			var logger = new Logger(output);
			logger.WriteLine("1");
			using(logger.Indent())
				logger.WriteLine("2");
			logger.WriteLine("3");
			Assert.AreEqual(string.Format("1{0}   2{0}3{0}", output.NewLine), output.ToString());
		}

		[Test]
		public void Write_should_support_formatting()
		{
			var output = new StringWriter();
			var logger = new Logger(output);
			logger.WriteLine("{0}+{1}", 1, 2);
			Assert.AreEqual("1+2" + output.NewLine, output.ToString());
		}
	}
}