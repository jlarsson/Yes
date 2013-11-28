using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class JsSyntaxError : JsSyntaxErrorPrototype
    {
        public JsSyntaxError(IEnvironment environment, IJsClass @class)
            : base(environment, @class)
        {
        }
    }
}