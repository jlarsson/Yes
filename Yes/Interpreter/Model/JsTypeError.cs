using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsTypeError : JsTypeErrorPrototype
    {
        public JsTypeError(IEnvironment environment, IJsClass @class)
            : base(environment, @class)
        {
        }
    }
}