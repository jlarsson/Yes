using System;
using System.Globalization;
using Yes.Interpreter.Model;

namespace Yes.Utility
{
    public static class Conversion
    {
        public static readonly IFormatProvider DoubleFormat = CultureInfo.GetCultureInfo("sv-SE");

        public static T Cast<T>(IJsValue value, bool @throw) where T : class
        {
            var v = value as T;
            if ((v == null) && @throw)
            {
                throw new ApplicationException();
            }
            return v;
        }
    }
}
