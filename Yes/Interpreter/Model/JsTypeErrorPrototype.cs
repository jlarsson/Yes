using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsTypeErrorPrototype : JsErrorPrototype
    {
        public JsTypeErrorPrototype(IEnvironment environment, IJsClass @class)
            : base(environment, @class)
        {
        }

        protected override string GetErrorName()
        {
            return "TypeError";
        }
    }
}