using System;
using System.Globalization;
using Yes.Interpreter.Model;
using Yes.Runtime.Error;

namespace Yes.Utility
{
    public static class BindParameters
    {
        public static T OfTypeOrNull<T>(IJsValue[] args, int index) where T : class, IJsValue
        {
            if (index >= args.Length)
            {
                return null;
            }
            var v = args[index];
            if (v is T)
            {
                return v as T;
            }
            if (v == null)
            {
                return null;
            }
            throw new JsTypeError();
        }
    }

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