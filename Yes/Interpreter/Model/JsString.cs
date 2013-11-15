using System;
using System.Globalization;
using Yes.Runtime.Environment;
using Yes.Runtime.Prototypes;
using Yes.Utility;

namespace Yes.Interpreter.Model
{
    public class JsString : JsObject, IJsString
    {
        public JsString(IEnvironment environment, IJsObject prototype, string value)
            : base(environment, prototype)
        {
            Value = value;
        }

        protected string Value { get; set; }

        public override JsTypeCode TypeCode
        {
            get { return JsTypeCode.String; }
        }

        public override int? ToArrayIndex()
        {
            double result;
            return double.TryParse(Value, NumberStyles.Number, Conversion.DoubleFormat, out result)
                       ? (int?)Math.Floor(result)
                       : null;
        }

        public override object ToPrimitive()
        {
            return Value;
        }

        public override bool ToBoolean()
        {
            return !string.IsNullOrEmpty(Value);
        }

        public override double ToNumber()
        {
            double result;
            return double.TryParse(Value, NumberStyles.Number, Conversion.DoubleFormat, out result)
                       ? result
                       : double.NaN;
        }

        public override int ToInteger()
        {
            var n = ToNumber();
            if (double.IsNaN(n))
            {
                return 0;
            }
            return (int)(Math.Sign(n) * Math.Floor(Math.Abs(n)));
        }

        public override string ToString()
        {
            return Value;
        }

        [JsInstanceProperty("length", Enumerable = false, Configurable = false)]
        public IJsValue JsLength
        {
            get { return Environment.CreateNumber((Value ?? "").Length); }
        }
    }
}