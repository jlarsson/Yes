using System.Collections.Generic;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class ObjectConstructor : JsConstructor
    {
        public ObjectConstructor(IEnvironment environment) : base(environment)
        {
        }

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            return new JsObject(Environment, null);
        }

        public static IJsValue Construct(IEnvironment environment)
        {
            return new JsObject(environment, null);
        }
    }
}