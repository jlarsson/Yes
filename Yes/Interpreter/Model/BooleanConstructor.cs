using System.Collections.Generic;
using System.Linq;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class BooleanConstructor : JsConstructor<JsBool>, IBooleanConstructor
    {
        public BooleanConstructor(IEnvironment environment)
            : base(environment, environment.Context.GetPrototype<BooleanConstructor>())
        {
        }

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            return Construct(arguments.Select(a => a.ToBoolean()).FirstOrDefault());
        }

        public IJsBool Construct(bool value)
        {
            return new JsBool(Environment, ClassPrototype, value);
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new BooleanConstructor(environment);
        }
    }
}