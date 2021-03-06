using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

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

        public IJsValue Evaluate(IEnvironment environment)
        {
            var lvalue = Lhs.ReferenceCast<ILValue>("Invalid left-hand side in assignment");
            return lvalue.SetValue(environment, Rhs.Evaluate(environment));
        }
    }
}