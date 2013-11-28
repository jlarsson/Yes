using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsReferenceErrorPrototype : JsErrorPrototype
    {
        public JsReferenceErrorPrototype(IEnvironment environment, IJsClass @class)
            : base(environment, @class)
        {
        }
        protected override string GetErrorName()
        {
            return "ReferenceError";
        }
    }
}