using System.Collections.Generic;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public abstract class JsConstructor: JsObject, IJsConstructor{
        protected JsConstructor(IEnvironment environment) : base(environment, null)
        {
        }

        public abstract IJsValue Construct(IEnumerable<IJsValue> arguments);
    }
}