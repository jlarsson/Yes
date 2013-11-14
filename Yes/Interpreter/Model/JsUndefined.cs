using Yes.Runtime;

namespace Yes.Interpreter.Model
{
    public class JsUndefined : IJsUndefined
    {
        public static JsUndefined Instance = new JsUndefined();

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
            return double.NaN;
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
            throw new System.NotImplementedException();
        }

        public IReference GetReference(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}