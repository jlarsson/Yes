using System;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsHostFunction : JsFunctionPrototype, IJsFunction
    {
        public JsHostFunction(IEnvironment environment, Func<IEnvironment, IJsValue, IJsValue[], IJsValue> func)
            : base(environment, null)
        {
            Func = func;
        }

        public Func<IEnvironment, IJsValue, IJsValue[], IJsValue> Func { get; protected set; }

        public override IJsValue Apply(IJsValue @this, params IJsValue[] arguments)
        {
            return Func(Environment, @this, arguments) ?? JsUndefined.Value;
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new JsHostFunction(environment,Func);
        }

        public override string ToString()
        {
            return "function () {/*native code*/}";
        }
    }
}