using Yes.Interpreter.Model;
using Yes.Runtime.Error;

namespace Yes.Runtime.Environment
{
    public class ReadonlyUndefinedPropertyReference : IReference
    {
        public string Name { get; protected set; }

        public ReadonlyUndefinedPropertyReference(string name)
        {
            Name = name;
        }

        public IJsValue GetValue(IJsValue self)
        {
            return JsUndefined.Value;
        }

        public IJsValue SetValue(IJsValue self, IJsValue value)
        {
            throw new JsReferenceException("Cannot set property {0}", Name);
        }
    }
}