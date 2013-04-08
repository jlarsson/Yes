using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public class Assign: IAst
    {
        public IAst Lhs { get; set; }
        public IAst Rhs { get; set; }

        public Assign(IAst lhs, IAst rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        public IJsValue Evaluate(IScope scope)
        {
            var lvalue = Lhs as ILValue;
            return lvalue.SetValue(scope, Rhs.Evaluate(scope));
        }
    }
}