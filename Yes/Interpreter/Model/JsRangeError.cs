using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsRangeError : JsRangeErrorPrototype
    {
        public JsRangeError(IEnvironment environment, IJsClass @class)
            : base(environment, @class)
        {
        }
    }
}