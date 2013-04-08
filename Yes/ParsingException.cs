using System;
using System.Text;

namespace Yes
{
    public class ParsingException : ApplicationException
    {
        public ParsingException(string message, string source, int sourcePosition)
            : base(FormatMessage(message, source, sourcePosition))
        {
        }

        private static string FormatMessage(string message, string source, int sourcePosition)
        {
            var sb = new StringBuilder();

            sb.AppendLine(message);

            var p = Math.Max(Math.Min(source.Length, sourcePosition), 0);
            sb.AppendLine(source.Substring(0, p));
            sb.Append("[Error is somehwere here]");
            sb.Append(source.Substring(p, source.Length - p));
            return sb.ToString();
        }
    }
}