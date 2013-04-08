using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class Break: IAst{
        public IJsValue Evaluate(IScope scope)
        {
            scope.Break = true;
            return scope.CreateUndefined();
        }
    }
}