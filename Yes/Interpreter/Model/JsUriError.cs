using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsUriError : JsUriErrorPrototype
    {
        public JsUriError(IEnvironment environment, IJsClass @class)
            : base(environment, @class)
        {
        }
    }
}