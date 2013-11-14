using Yes.Interpreter.Model;

namespace Yes.Runtime.Environment
{
    public class MissingScopeReference : IReference
    {
        public IJsValue GetValue()
        {
            return JsUndefined.Instance;
        }

        public IJsValue SetValue(IJsValue value)
        {
            throw new JsException();
        }
    }
}