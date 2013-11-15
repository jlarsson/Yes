using Yes.Runtime;

namespace Yes.Interpreter.Model
{
    public class JsNull : IJsNull
    {
        public static readonly JsNull Value = new JsNull();

        public JsTypeCode TypeCode
        {
            get { return JsTypeCode.Null; }
        }

        public IReference GetReference(IJsValue name)
        {
            throw new JsReferenceError();
        }

        public IReference GetReference(string name)
        {
            throw new JsReferenceError();
        }

        public int? ToArrayIndex()
        {
            return null;
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