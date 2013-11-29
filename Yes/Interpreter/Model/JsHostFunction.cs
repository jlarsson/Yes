using System;
using System.Collections.Generic;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsHostFunction : JsFunctionPrototype
    {
        public JsHostFunction(IEnvironment environment, Func<IEnvironment, IJsValue, IList<IJsValue>, IJsValue> func)
            : base(environment, null)
        {
            Func = func;
        }

        public Func<IEnvironment, IJsValue, IList<IJsValue>, IJsValue> Func { get; protected set; }

        public override IJsValue Apply(IJsValue @this, IList<IJsValue> arguments)
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