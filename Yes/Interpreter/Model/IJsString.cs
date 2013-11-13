using System;
using System.Globalization;
using Yes.Utility;

namespace Yes.Interpreter.Model
{
    public interface IJsString{}

    public class JsString : AbstractJsValue, IJsString
    {
        public JsString(IScope scope, string value) : base(scope)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public string Value { get; protected set; }

        public override IJsValue Prototype
        {
            get { return Scope.ProtoTypes.String;  }
        }

        public override JsTypeCode TypeCode
        {
            get { return JsTypeCode.String; }
        }

        public override bool IsTruthy()
        {
            return !string.IsNullOrEmpty(Value);
        }

        public override bool IsFalsy()
        {
            return string.IsNullOrEmpty(Value);
        }

        public override int? TryEvaluateToIndex()
        {
            double d;
            return double.TryParse(Value, NumberStyles.Number, Conversion.DoubleFormat, out d) ? (int?) d : null;
        }

        public static IJsValue CreatePrototype(Scope scope)
        {
            return new JsPrototype(scope);
        }
    }
}