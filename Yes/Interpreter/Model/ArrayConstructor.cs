using System.Collections.Generic;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class ArrayConstructor: JsConstructor
    {
        public ArrayConstructor(IEnvironment environment) : base(environment)
        {
        }

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            return Construct(Environment, arguments);
        }

        public static IJsValue Construct(IEnvironment environment, IEnumerable<IJsValue> arguments)
        {
            return new JsArray(environment, null, arguments);
        }
    }
}