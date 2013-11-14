using System;
using Yes.Runtime;

namespace Yes.Interpreter.Model
{
    public class JsNumber : IJsNumber
    {
        public JsNumber(double value) : base()
        {
            Value = value;
        }

        public double Value { get; set; }

        public JsTypeCode TypeCode
        {
            get { return JsTypeCode.Number; }
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
            return (int)Math.Floor(Value);
        }

        public bool ToBoolean()
        {
            if (double.IsNaN(Value))
            {
                return false;
            }
            return (Value > double.Epsilon) || (Value < -double.Epsilon);
        }

        public double ToNumber()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}