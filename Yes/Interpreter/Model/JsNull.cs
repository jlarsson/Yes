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
            throw new System.NotImplementedException();
        }

        public IReference GetReference(string name)
        {
            throw new System.NotImplementedException();
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
    }
}