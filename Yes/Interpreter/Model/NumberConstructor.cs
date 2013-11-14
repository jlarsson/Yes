using System.Collections.Generic;
using System.Linq;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class NumberConstructor: JsConstructor{
        public NumberConstructor(IEnvironment environment) : base(environment)
        {
        }

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            return new JsNumber(arguments.Select(a => a.ToNumber()).FirstOrDefault());
        }

        public static IJsValue Construct(double value)
        {
            return new JsNumber(value);
        }
    }
}