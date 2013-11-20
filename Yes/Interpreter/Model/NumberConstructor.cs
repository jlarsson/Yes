using System.Collections.Generic;
using System.Linq;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class NumberConstructor: JsConstructorFunction<JsNumber>, INumberConstructor{
        public NumberConstructor(IEnvironment environment, IJsClass @class, IJsClass constructedClass)
            : base(environment, @class, constructedClass)
        {
        }

        public override string ToString()
        {
            return "[Function: Number]";
        }

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            return Construct(arguments.Select(a => a.ToNumber()).FirstOrDefault());
        }

        public IJsNumber Construct(double value)
        {
            return new JsNumber(Environment,ConstructedClass, value);
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new NumberConstructor(environment, Class, ConstructedClass);
        }
    }
}