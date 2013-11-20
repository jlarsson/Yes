using System.Collections.Generic;
using System.Linq;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class BooleanConstructor : JsConstructorFunction, IBooleanConstructor
    {
        public BooleanConstructor(IEnvironment environment, IJsClass @class, IJsClass constructedClass)
            : base(environment, @class, constructedClass)
        {
        }

        public override string ToString()
        {
            return "[Function: Boolean]";
        }

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            return Construct(arguments.Select(a => a.ToBoolean()).FirstOrDefault());
        }

        public IJsBool Construct(bool value)
        {
            return new JsBoolean(Environment, ConstructedClass, value);
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new BooleanConstructor(environment, Class, ConstructedClass);
        }
    }
}