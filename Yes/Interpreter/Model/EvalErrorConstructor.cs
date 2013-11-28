using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class EvalErrorConstructor : AbstractErrorConstructor
    {
        public EvalErrorConstructor(IEnvironment environment, IJsClass @class, IJsClass constructedClass)
            : base(environment, @class, constructedClass)
        {
        }

        public override IJsObject Construct(string message, string fileName, int lineNumber)
        {
            return new JsEvalError(Environment, ConstructedClass) { Message = message, FileName = fileName, LineNumber = lineNumber };
        }
    }
}