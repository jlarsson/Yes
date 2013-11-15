using System;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsHostFunction : JsObject, IJsFunction
    {
        public JsHostFunction(IEnvironment environment, Func<IEnvironment, IJsValue, IJsValue[], IJsValue> func)
            : base(environment, null)
        {
            Func = func;
        }

        public Func<IEnvironment, IJsValue, IJsValue[], IJsValue> Func { get; protected set; }

        public override JsTypeCode TypeCode
        {
            get { return JsTypeCode.Function; }
        }

        public IJsValue Apply(IJsValue @this, params IJsValue[] arguments)
        {
            return Func(Environment, @this, arguments) ?? JsUndefined.Value;
        }
    }
}