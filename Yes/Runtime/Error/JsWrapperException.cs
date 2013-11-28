using System;
using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Runtime.Error
{
    public class JsWrapperException: JsException
    {
        public IJsValue Value { get; protected set; }

        public JsWrapperException(IJsValue value)
        {
            Value = value;
        }

        public override IJsValue ToJsValue(IEnvironment environment)
        {
            return Value;
        }
    }
}