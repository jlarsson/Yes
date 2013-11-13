using System;

namespace Yes.Interpreter.Model
{
    public class JsHostFunction : JsCommonObject, IJsFunction
    {
        public JsHostFunction(IScope scope, Func<IScope, IJsValue, IJsValue[], IJsValue> func): base(scope)
        {
            Scope = scope;
            Func = func;
        }

        public Func<IScope, IJsValue, IJsValue[], IJsValue> Func { get; set; }

        #region IJsFunction Members

        public override JsTypeCode TypeCode
        {
            get { return JsTypeCode.Function; }
        }

        public IJsValue Apply(IJsValue @this, params IJsValue[] arguments)
        {
            return Func(Scope, @this, arguments) ?? Scope.CreateUndefined();
        }

        #endregion
    }
}