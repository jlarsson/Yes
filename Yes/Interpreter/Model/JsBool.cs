namespace Yes.Interpreter.Model
{
    public class JsBool : AbstractJsValue, IJsBool
    {
        public JsBool(IScope scope, bool value) : base(scope)
        {
            Value = value;
        }

        public bool Value { get; protected set; }

        public override IJsValue Prototype
        {
            get { return Scope.ProtoTypes.Bool; }
        }

        #region IJsBool Members

        public override JsTypeCode TypeCode
        {
            get { return JsTypeCode.Bool; }
        }

        public override bool IsTruthy()
        {
            return Value;
        }

        public override bool IsFalsy()
        {
            return !Value;
        }

        #endregion

        public static IJsValue CreatePrototype(IScope scope)
        {
            var prototype = new JsPrototype(scope);
            return prototype;
        }
    }
}