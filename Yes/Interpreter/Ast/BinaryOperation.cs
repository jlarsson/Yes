using Yes.Interpreter.Model;
using Yes.Runtime.Environment;
using Yes.Runtime.Operators;

namespace Yes.Interpreter.Ast
{
    public class BinaryOperation: IAst
    {
        public IBinaryOperator Operator { get; protected set; }
        public IAst Lhs { get; protected set; }
        public IAst Rhs { get; protected set; }

        public BinaryOperation(IBinaryOperator @operator, IAst lhs, IAst rhs)
        {
            Operator = @operator;
            Lhs = lhs;
            Rhs = rhs;
        }

        public IJsValue Evaluate(IEnvironment environment)
        {
            var l = Lhs.Evaluate(environment);
            var r = Rhs.Evaluate(environment);

            return Operator.Evaluate(environment, l, r);
        }
    }
}