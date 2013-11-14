using System;
using System.Globalization;
using Yes.Runtime;
using Yes.Utility;

namespace Yes.Interpreter.Model
{
    public class JsString : IJsString
    {
        public JsString(string value)
        {
            Value = value;
        }

        protected string Value { get; set; }

        public JsTypeCode TypeCode
        {
            get { return JsTypeCode.String; }
        }

        public IReference GetReference(IJsValue name)
        {
            throw new NotImplementedException();
        }

        public IReference GetReference(string name)
        {
            throw new NotImplementedException();
        }

        public int? ToArrayIndex()
        {
            double result;
            return double.TryParse(Value, NumberStyles.Number, Conversion.DoubleFormat, out result)
                       ? (int?)Math.Floor(result)
                       : null;
        }

        public bool ToBoolean()
        {
            return !string.IsNullOrEmpty(Value);
        }

        public double ToNumber()
        {
            double result;
            return double.TryParse(Value, NumberStyles.Number, Conversion.DoubleFormat, out result)
                       ? result
                       : double.NaN;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}