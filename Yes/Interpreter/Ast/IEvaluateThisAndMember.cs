using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public interface IEvaluateThisAndMember
    {
        IJsValue Evaluate(IScope scope, out IJsValue @this);
    }
}