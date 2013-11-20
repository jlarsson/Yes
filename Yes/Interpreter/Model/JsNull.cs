using Yes.Runtime;
using Yes.Runtime.Error;

namespace Yes.Interpreter.Model
{
    public class JsNull : IJsNull
    {
        public static readonly JsNull Value = new JsNull();

        public IReference GetReference(IJsValue name)
        {
            throw new JsReferenceError("Cannot read property {0} of {1}", name, this);
        }

        public IReference GetReference(string name)
        {
            throw new JsReferenceError("Cannot read property {0} of {1}", name, this);
        }

        public int? ToArrayIndex()
        {
            return null;
        }

        public JsVariant ToVariant()
        {
            return new JsVariant(this);
        }

        public object ToPrimitive()
        {
            return this;
        }

        public bool ToBoolean()
        {
            return false;
        }

        public double ToNumber()
        {
            return 0;
        }

        public int ToInteger()
        {
            return 0;
        }

        public override string ToString()
        {
            return "null";
        }
    }
}