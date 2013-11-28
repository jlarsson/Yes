using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsEvalError: JsEvalErrorPrototype{
        public JsEvalError(IEnvironment environment, IJsClass @class) : base(environment, @class)
        {
        }
    }
}