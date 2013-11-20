using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsObject: JsObjectPrototype
    {
        public JsObject(IEnvironment environment, IJsClass @class) : base(environment, @class)
        {
        }
    }
}