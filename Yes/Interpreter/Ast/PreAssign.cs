using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class PreAssign : IAst
    {
        public IAst Lhs { get; set; }
        public IAst Rhs { get; set; }

        public PreAssign(IAst lhs, IAst rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        public IJsValue Evaluate(IEnvironment environment)
        {
            var lvalue = Lhs.ReferenceCast<ILValue>("Invalid left-hand side expression in prefix operation");
            return lvalue.SetValue(environment, Rhs.Evaluate(environment));
        }
    }
}