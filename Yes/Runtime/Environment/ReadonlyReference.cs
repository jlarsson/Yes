using Yes.Interpreter.Model;
using Yes.Runtime.Error;

namespace Yes.Runtime.Environment
{
    public class ReadonlyReference : IReference
    {
        private readonly IReference _inner;

        public ReadonlyReference()
        {
        }

        public ReadonlyReference(IReference inner)
        {
            _inner = inner;
        }

        public IJsValue GetValue(IJsValue self)
        {
            return _inner == null ? JsUndefined.Value : _inner.GetValue(self);
        }

        public IJsValue SetValue(IJsValue self, IJsValue value)
        {
            throw new JsReferenceError();
        }
    }
}