using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public interface IAst
    {
        IJsValue Evaluate(IScope scope);
    }
}