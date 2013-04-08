namespace Yes.Interpreter.Model
{
    public class JsUndefined : AbstractJsValue, IJsUndefined
    {
        public JsUndefined(IScope scope) : base(scope)
        {
        }

        #region IJsUndefined Members

        public override IJsValue Prototype
        {
            get { return Scope.ProtoTypes.Undefined; }
        }

        public override JsTypeCode TypeCode
        {
            get { return JsTypeCode.Undefined; }
        }

        public override bool IsTruthy()
        {
            return false;
        }

        public override bool IsFalsy()
        {
            return true;
        }

        #endregion
    }
}