using System.Collections.Generic;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public abstract class AbstractErrorConstructor : JsConstructorFunction, IErrorConstructor
    {
        protected AbstractErrorConstructor(IEnvironment environment, IJsClass @class, IJsClass constructedClass)
            : base(environment, @class, constructedClass)
        {
        }

        public override IJsValue Construct(IList<IJsValue> arguments)
        {
            var message = arguments.Count > 0 ? arguments[0].ToString() : "";
            var fileName = arguments.Count > 1 ? arguments[1].ToString() : "";
            var lineNumber = arguments.Count > 2 ? arguments[2].ToInteger() : 0;
            return Construct(message, fileName, lineNumber);
        }

        public abstract IJsObject Construct(string message, string fileName, int lineNumber);
    }
}