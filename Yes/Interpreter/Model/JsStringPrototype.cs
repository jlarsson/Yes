using System;
using System.Globalization;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;
using Yes.Utility;

namespace Yes.Interpreter.Model
{
    public abstract class JsStringPrototype: JsObjectPrototype, IJsString{
        protected JsStringPrototype(IEnvironment environment, IJsClass @class) : base(environment, @class)
        {
        }

        public abstract string Value { get; }

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
    }
}