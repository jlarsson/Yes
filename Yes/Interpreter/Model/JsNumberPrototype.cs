using System;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;
using Yes.Utility;

namespace Yes.Interpreter.Model
{
    public abstract class JsNumberPrototype: JsObjectPrototype, IJsNumber
    {
        protected JsNumberPrototype(IEnvironment environment, IJsClass @class)
            : base(environment, @class)
        {
        }

        public abstract double Value { get;  }

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
            return (int)(Math.Sign(Value) * Math.Floor(Math.Abs(Value)));
        }

        public override string ToString()
        {
            return Value.ToString(Conversion.DoubleFormat);
        }
    }
}