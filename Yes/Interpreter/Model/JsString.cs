using System;
using System.Globalization;
using Yes.Runtime.Environment;
using Yes.Runtime.Prototypes;
using Yes.Utility;

namespace Yes.Interpreter.Model
{
    public class JsString : JsObject, IJsString
    {
        public JsString(IEnvironment environment, IJsClass @class, string value)
            : base(environment, @class)
        {
            Value = value;
        }

        public string Value { get; protected set; }

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

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new JsString(environment,Class,Value);
        }

        [JsInstanceMember("length", Enumerable = false, Configurable = false)]
        public IJsValue JsLength
        {
            get { return Environment.CreateNumber((Value ?? "").Length); }
        }
    }
}