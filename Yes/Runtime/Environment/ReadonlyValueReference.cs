using Yes.Interpreter.Model;
using Yes.Runtime.Error;

namespace Yes.Runtime.Environment
{
    public class ReadonlyValueReference: IReference
    {
        public IJsValue Value { get; protected set; }

        public ReadonlyValueReference(IJsValue value)
        {
            Value = value;
        }

        public IJsValue GetValue(IJsValue self = null)
        {
            return Value;
        }

        public IJsValue SetValue(IJsValue self, IJsValue value)
        {
            throw new JsReferenceException("Illegal assignment");
        }
    }
}