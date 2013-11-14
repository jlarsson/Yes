using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public interface IEvaluateThisAndMember
    {
        IJsValue Evaluate(IEnvironment environment, out IJsValue @this);
    }
}