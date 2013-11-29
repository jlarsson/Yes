using System.Collections.Generic;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsArgumentsConstructor : JsConstructorFunction, IArgumentsConstructor {
        public JsArgumentsConstructor(IEnvironment environment, IJsClass @class, IJsClass constructedClass) : base(environment, @class, constructedClass)
        {
        }

        public override IJsValue Construct(IList<IJsValue> arguments)
        {
            return Construct(Environment, arguments);
        }

        public IJsArguments Construct(IEnvironment callEnvironment, IList<IJsValue> arguments)
        {
            return new JsArguments(Environment, callEnvironment, ConstructedClass, arguments);
        }
    }
}