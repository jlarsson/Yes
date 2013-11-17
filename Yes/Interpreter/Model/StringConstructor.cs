using System.Collections.Generic;
using System.Linq;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class StringConstructor : JsConstructor<JsString>, IStringConstructor
    {
        public StringConstructor(IEnvironment environment)
            : base(environment, environment.Context.GetPrototype<StringConstructor>())
        {
        }

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            return Construct(arguments.Select(a => a.ToString()).FirstOrDefault() ?? "");
        }

        public IJsString Construct(string value)
        {
            return new JsString(Environment, ClassPrototype, value);
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new StringConstructor(environment);
        }
    }
}