using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsEvalErrorPrototype: JsErrorPrototype{
        protected JsEvalErrorPrototype(IEnvironment environment, IJsClass @class) : base(environment, @class)
        {
        }
        protected override string GetErrorName()
        {
            return "EvalError";
        }
    }
}