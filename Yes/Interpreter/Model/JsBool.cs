using System;
using Yes.Runtime;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsBool : IJsBool
    {
        public JsBool(bool value)
        {
            Value = value;
        }

        public bool Value { get; protected set; }


        public JsTypeCode TypeCode
        {
            get { return JsTypeCode.Bool; }
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
            return Value ? 1 : 0;
        }

        public bool ToBoolean()
        {
            return Value;
        }

        public double ToNumber()
        {
            return Value ? 1d : 0d;
        }
    }
}