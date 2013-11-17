using System.Collections.Generic;
using System.Linq;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class NumberConstructor: JsConstructor<JsNumber>, INumberConstructor{
        public NumberConstructor(IEnvironment environment)
            : base(environment, environment.Context.GetPrototype<NumberConstructor>())
        {
        }

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            return Construct(arguments.Select(a => a.ToNumber()).FirstOrDefault());
        }

        public IJsNumber Construct(double value)
        {
            return new JsNumber(Environment,ClassPrototype, value);
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new NumberConstructor(environment);
        }
    }
}