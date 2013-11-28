using Yes.Runtime.Classes;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class ErrorConstructor: AbstractErrorConstructor{
        public ErrorConstructor(IEnvironment environment, IJsClass @class, IJsClass constructedClass) : base(environment, @class, constructedClass)
        {
        }

        public override IJsObject Construct(string message, string fileName, int lineNumber)
        {
            return new JsError(Environment, ConstructedClass)
                       {Message = message, FileName = fileName, LineNumber = lineNumber};
        }
    }
}