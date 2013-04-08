using System;

namespace Yes.Interpreter.Model
{
    public class JsNumber : AbstractJsValue, IJsNumber
    {
        public JsNumber(IScope scope, double value) : base(scope)
        {
            Value = value;
        }

        #region IJsNumber Members

        public double Value { get; set; }

        public override IJsValue Prototype
        {
            get { return Scope.ProtoTypes.Number; }
        }

        public override JsTypeCode TypeCode
        {
            get { return JsTypeCode.Number; }
        }

        public override bool IsTruthy()
        {
            return Math.Abs(Value - 0) > Double.Epsilon;
        }

        public override bool IsFalsy()
        {
            return !IsFalsy();
        }

        #endregion

        public override string ToString()
        {
            return Value.ToString();
        }

        public static IJsValue CreatePrototype(Scope scope)
        {
            var prototype = new JsPrototype(scope);
            return prototype;
        }
    }
}