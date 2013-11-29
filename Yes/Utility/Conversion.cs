using System;
using System.Collections.Generic;
using System.Globalization;
using Yes.Interpreter.Model;
using Yes.Runtime.Error;

namespace Yes.Utility
{
    public static class BindParameters
    {
        public static T OfTypeOrNull<T>(IList<IJsValue> args, int index) where T : class, IJsValue
        {
            if (index >= args.Count)
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
            throw new JsTypeException();
        }
    }

    public static class Conversion
    {
        public static readonly IFormatProvider DoubleFormat = CultureInfo.GetCultureInfo("en-US");

        public static double ParseDouble(string text)
        {
            return double.Parse(text, DoubleFormat);
        }


        public static int? ToArrayIndex(IJsUndefined value)
        {
            return null;
        }
        public static object ToPrimitive(IJsUndefined value)
        {
            return value;
        }
        public static bool ToBoolean(IJsUndefined value)
        {
            return false;
        }
        public static double ToNumber(IJsUndefined value)
        {
            return double.NaN;
        }
        public static int ToInteger(IJsUndefined value)
        {
            return 0;
        }
        public static string ToString(IJsUndefined value)
        {
            return "undefined";
        }


        public static int? ToArrayIndex(JsNull value)
        {
            return null;
        }

        public static object ToPrimitive(JsNull value)
        {
            return value;
        }

        public static bool ToBoolean(JsNull value)
        {
            return false;
        }

        public static double ToNumber(JsNull value)
        {
            return 0d;
        }

        public static int ToInteger(JsNull value)
        {
            return 0;
        }

        public static string ToString(JsNull value)
        {
            return "null";
        }

        public static int? ToArrayIndex(bool value)
        {
            return value ? 1 : 0;
        }

        public static object ToPrimitive(bool value)
        {
            return value;
        }

        public static bool ToBoolean(bool value)
        {
            return value;
        }

        public static double ToNumber(bool value)
        {
            return value ? 1d : 0d;
        }

        public static int ToInteger(bool value)
        {
            return value ? 1 : 0;
        }

        public static string ToString(bool value)
        {
            return value ? "true" : "false";
        }

        public static int? ToArrayIndex(double value)
        {
            return (int)Math.Floor(value);
        }

        public static object ToPrimitive(double value)
        {
            return value;
        }

        public static bool ToBoolean(double value)
        {
            return !double.IsNaN(value) && ((value > double.Epsilon) || (value < -double.Epsilon));
        }

        public static double ToNumber(double value)
        {
            return value;
        }

        public static int ToInteger(double value)
        {
            return (int)(Math.Sign(value) * Math.Floor(Math.Abs(value)));
        }

        public static string ToString(double value)
        {
            return value.ToString(DoubleFormat);
        }
 
        public static int? ToArrayIndex(int value)
        {
            return value;
        }

        public static object ToPrimitive(int value)
        {
            return value;
        }

        public static bool ToBoolean(int value)
        {
            return value != 0;
        }

        public static double ToNumber(int value)
        {
            return value;
        }

        public static int ToInteger(int value)
        {
            return value;
        }

        public static string ToString(int value)
        {
            return value.ToString(DoubleFormat);
        }

        public static int? ToArrayIndex(IJsObject value)
        {
            return null;
        }

        public static object ToPrimitive(IJsObject value)
        {
            return value;
        }

        public static bool ToBoolean(IJsObject value)
        {
            return true;
        }

        public static double ToNumber(IJsObject value)
        {
            return double.NaN;
        }

        public static int ToInteger(IJsObject value)
        {
            return 0;
        }

        public static string ToString(IJsObject value)
        {
            return "[object Object]";
        }

        public static int? ToArrayIndex(string value)
        {
            double result;
            return double.TryParse(value, NumberStyles.Number, DoubleFormat, out result)
                       ? (int?)Math.Floor(result)
                       : null;
        }

        public static object ToPrimitive(string value)
        {
            return value;
        }

        public static bool ToBoolean(string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static double ToNumber(string value)
        {
            double result;
            return double.TryParse(value, NumberStyles.Number, DoubleFormat, out result)
                       ? result
                       : double.NaN;

        }

        public static int ToInteger(string value)
        {
            var n = ToNumber(value);
            if (double.IsNaN(n))
            {
                return 0;
            }
            return (int)(Math.Sign(n) * Math.Floor(Math.Abs(n)));
        }

        public static string ToString(string value)
        {
            return value;
        }
    }
}