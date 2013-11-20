using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public class PostAssign : IAst
    {
        public IAst Lhs { get; set; }
        public IAst Rhs { get; set; }

        public PostAssign(IAst lhs, IAst rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        public IJsValue Evaluate(IEnvironment environment)
        {
            var result = Lhs.Evaluate(environment);
            var lvalue = Lhs.Cast<ILValue>();
            lvalue.SetValue(environment, Rhs.Evaluate(environment));
            return result;
        }
    }
}