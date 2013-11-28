using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class SyntaxErrorConstructor : AbstractErrorConstructor
    {
        public SyntaxErrorConstructor(IEnvironment environment, IJsClass @class, IJsClass constructedClass)
            : base(environment, @class, constructedClass)
        {
        }

        public override IJsObject Construct(string message, string fileName, int lineNumber)
        {
            return new JsSyntaxError(Environment, ConstructedClass) { Message = message, FileName = fileName, LineNumber = lineNumber };
        }
    }
}