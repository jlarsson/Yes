namespace Yes.Interpreter.Model
{
    public class JsNull : IJsNull
    {
        public static readonly JsNull Value = new JsNull();

        #region IJsNull Members

        public IJsObjectMembers Members
        {
            get
            {
                JsError.Throw("Illegal member access on null object.");
                return null;
            }
        }

        public JsTypeCode TypeCode
        {
            get { return JsTypeCode.Null; }
        }

        public bool IsTruthy()
        {
            return false;
        }

        public bool IsFalsy()
        {
            return true;
        }

        #endregion
    }
}