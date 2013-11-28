using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsUriErrorPrototype : JsErrorPrototype
    {
        public JsUriErrorPrototype(IEnvironment environment, IJsClass @class)
            : base(environment, @class)
        {
        }

        protected override string GetErrorName()
        {
            return "UriError";
        }
    }
}