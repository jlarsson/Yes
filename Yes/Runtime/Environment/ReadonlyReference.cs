using Yes.Interpreter.Model;

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

        public IJsValue GetValue()
        {
            return _inner == null ? JsUndefined.Instance : _inner.GetValue();
        }

        public IJsValue SetValue(IJsValue value)
        {
            throw new JsReferenceError();
        }
    }
}