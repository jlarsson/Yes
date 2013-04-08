using System;

namespace Yes.Interpreter
{
    public static class Error
    {
        public static void Throw(string format, params object[] args)
        {
            throw new ApplicationException(string.Format(format, args));
        }
    }
}