using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsReferenceError : JsReferenceErrorPrototype
    {
        public JsReferenceError(IEnvironment environment, IJsClass @class)
            : base(environment, @class)
        {
        }
    }
}