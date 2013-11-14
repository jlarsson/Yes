using Yes.Interpreter.Model;

namespace Yes.Runtime.Environment
{
    public class ValueReference : IReference
    {
        public IJsValue Value { get; protected set; }

        public ValueReference(IJsValue value)
        {
            Value = value;
        }

        public IJsValue GetValue()
        {
            return Value;
        }

        public IJsValue SetValue(IJsValue value)
        {
            return Value = value;
        }
    }
}