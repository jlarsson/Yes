using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsError: JsErrorPrototype{
        public JsError(IEnvironment environment, IJsClass @class) : base(environment, @class)
        {
        }

        protected override string GetErrorName()
        {
            return "Error";
        }
    }
}