using System.Collections.Generic;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class ObjectConstructor : JsConstructor, IObjectConstructor
    {
        public ObjectConstructor(IEnvironment environment) : base(environment)
        {
        }

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            return new JsObject(Environment, null);
        }

        public IJsObject Construct(IEnvironment environment)
        {
            return new JsObject(environment, ClassPrototype);
        }

        protected override IJsObject CreatePrototype()
        {
            return CreateProtypeForImplementation<JsObject>(null);
        }
    }
}