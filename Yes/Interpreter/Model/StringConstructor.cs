using System.Collections.Generic;
using System.Linq;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class StringConstructor : JsConstructor, IStringConstructor
    {
        public StringConstructor(IEnvironment environment) : base(environment)
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

        protected override IJsObject CreatePrototype()
        {
            return CreateProtypeForImplementation<JsString>(base.CreatePrototype());
        }
    }
}