using Yes.Interpreter.Model;

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