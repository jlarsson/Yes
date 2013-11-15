using System.Collections.Generic;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class ArrayConstructor: JsConstructor, IArrayConstructor
    {
        public ArrayConstructor(IEnvironment environment) : base(environment)
        {
        }

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            return new JsArray(Environment, ClassPrototype, arguments);
        }

        protected override IJsObject CreatePrototype()
        {
            return CreateProtypeForImplementation<JsArray>(base.CreatePrototype());
        }
    }
}