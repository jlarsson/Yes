using Yes.Interpreter.Model;
using Yes.Runtime.Error;

namespace Yes.Runtime.Environment
{
    public class MissingScopeReference : IReference
    {
        public IJsValue GetValue(IJsValue self)
        {
            return JsUndefined.Value;
        }

        public IJsValue SetValue(IJsValue self, IJsValue value)
        {
            throw new JsReferenceError();
        }
    }
}