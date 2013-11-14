using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public interface IAst
    {
        IJsValue Evaluate(IEnvironment environment);
    }
}