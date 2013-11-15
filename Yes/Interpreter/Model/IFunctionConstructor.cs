using Yes.Interpreter.Ast;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public interface IFunctionConstructor: IJsConstructor
    {
        IJsFunction Construct(IEnvironment environment, string name, string[] argumentNames, IAst body);
    }
}