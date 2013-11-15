using Yes.Runtime;

namespace Yes.Interpreter.Model
{
    public class JsUndefined : IJsUndefined
    {
        public static JsUndefined Value = new JsUndefined();

        public int? ToArrayIndex()
        {
            return null;
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
            return double.NaN;
        }

        public int ToInteger()
        {
            return 0;
        }

        public override string ToString()
        {
            return "undefined";
        }

        public JsTypeCode TypeCode
        {
            get { return JsTypeCode.Undefined; }
        }

        public IReference GetReference(IJsValue name)
        {
            throw new JsReferenceError();
        }

        public IReference GetReference(string name)
        {
            throw new JsReferenceError();
        }
    }
}