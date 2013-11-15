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

        public IJsValue GetValue(IJsValue self)
        {
            return Value;
        }

        public IJsValue SetValue(IJsValue self, IJsValue value)
        {
            return Value = value;
        }
    }
}