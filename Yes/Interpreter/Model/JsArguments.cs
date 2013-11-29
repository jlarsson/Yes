using System.Collections.Generic;
using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsArguments : JsArgumentsPrototype
    {
        public JsArguments(IEnvironment environment, IEnvironment callEnvironment, IJsClass @class, IList<IJsValue> arguments)
            : base(environment, callEnvironment, @class, arguments)
        {
        }
    }
}