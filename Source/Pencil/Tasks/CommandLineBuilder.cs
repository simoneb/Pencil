using System.Text;

namespace Pencil.Tasks
{
    public class CommandLineBuilder
    {
        private readonly StringBuilder inner;

        public CommandLineBuilder()
        {
            inner = new StringBuilder();
        }

        private static string Quote<T>(T value)
        {
            if (value == null)
                return null;

            var toString = value.ToString();

            return toString.Contains(" ") ? string.Format("\"{0}\"", toString) : toString;
        }

        public CommandLineBuilder Append(string value)
        {
            inner.AppendFormat(" {0}", Quote(value));
            return this;
        }

        public CommandLineBuilder Append<T>(string switchName, T switchValue)
        {
            inner.AppendFormat(" /{0}:{1}", switchName, Quote(switchValue));
            return this;
        }

        public override string ToString()
        {
            return inner.ToString();
        }
    }
}