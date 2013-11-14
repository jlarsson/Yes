using System.Collections.Generic;
using System.Linq;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class BooleanConstructor: JsConstructor{
        public BooleanConstructor(IEnvironment environment) : base(environment)
        {
        }

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            return Construct(arguments.Select(a => a.ToBoolean()).FirstOrDefault());
        }

        public static IJsValue Construct(bool value)
        {
            return new JsBool(value);
        }
    }
}