using System.Collections.Generic;
using System.Linq;
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

        public override IJsValue Construct(IEnumerable<IJsValue> arguments)
        {
            var args = arguments.ToList();
            var message = args.Count > 0 ? args[0].ToString() : "";
            var fileName = args.Count > 1 ? args[1].ToString() : "";
            var lineNumber = args.Count > 2 ? args[2].ToInteger() : 0;
            return Construct(message, fileName, lineNumber);
        }

        public abstract IJsObject Construct(string message, string fileName, int lineNumber);
    }
}