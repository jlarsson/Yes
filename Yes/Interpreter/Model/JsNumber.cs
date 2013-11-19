using System;
using Yes.Runtime.Environment;
using Yes.Utility;

namespace Yes.Interpreter.Model
{
    public class JsNumber : JsObject, IJsNumber
    {
        public JsNumber(IEnvironment environment, IJsClass @class, double value)
            : base(environment, @class)
        {
            Value = value;
        }

        public double Value { get; set; }

        public override int? ToArrayIndex()
        {
            return (int)Math.Floor(Value);
        }

        public override object ToPrimitive()
        {
            return Value;
        }

        public override bool ToBoolean()
        {
            if (double.IsNaN(Value))
            {
                return false;
            }
            return (Value > double.Epsilon) || (Value < -double.Epsilon);
        }

        public override double ToNumber()
        {
            return Value;
        }

        public override int ToInteger()
        {
            return (int)(Math.Sign(Value)*Math.Floor(Math.Abs(Value)));
        }

        public override string ToString()
        {
            return Value.ToString(Conversion.DoubleFormat);
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new JsNumber(environment,Class,Value);
        }
    }
}