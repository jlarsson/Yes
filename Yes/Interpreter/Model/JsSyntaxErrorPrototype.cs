using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsSyntaxErrorPrototype : JsErrorPrototype
    {
        public JsSyntaxErrorPrototype(IEnvironment environment, IJsClass @class)
            : base(environment, @class)
        {
        }
        protected override string GetErrorName()
        {
            return "SyntaxError";
        }
    }
}