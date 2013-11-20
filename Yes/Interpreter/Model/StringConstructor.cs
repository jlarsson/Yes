using System.Collections.Generic;
using System.Linq;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class StringConstructor : JsConstructorFunction<JsString>, IStringConstructor
    {
        public StringConstructor(IEnvironment environment, IJsClass @class, IJsClass constructedClass)
            : base(environment, @class, constructedClass)
        {
        }

        public override string ToString()
        {
            return "[Function: String]";
        }

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            return Construct(arguments.Select(a => a.ToString()).FirstOrDefault() ?? "");
        }

        public IJsString Construct(string value)
        {
            return new JsString(Environment, ConstructedClass, value);
        }

        public override IJsValue CloneTo(IEnvironment environment)
        {
            return new StringConstructor(environment, Class, ConstructedClass);
        }
    }
}