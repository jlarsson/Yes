using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsRangeErrorPrototype : JsErrorPrototype
    {
        public JsRangeErrorPrototype(IEnvironment environment, IJsClass @class)
            : base(environment, @class)
        {
        }
        protected override string GetErrorName()
        {
            return "RangeError";
        }
    }
}