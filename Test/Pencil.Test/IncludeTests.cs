using System.IO;

namespace Pencil.Test
{
    public class SpyLogger : Logger
    {
        public SpyLogger() : base(new StringWriter())
        {
        }

        public string Written { get { return ((StringWriter) Target).GetStringBuilder().ToString(); } }
    }
}