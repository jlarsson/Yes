using Yes.Interpreter.Model;
using Yes.Runtime.Environment;
using Yes.Runtime.Operators;

namespace Yes.Interpreter.Ast
{
    public class UnaryOperation: IAst
    {
        public IUnaryOperator Operator { get; protected set; }
        public IAst Value { get; protected set; }

        public UnaryOperation(IUnaryOperator @operator, IAst value)
        {
            Operator = @operator;
            Value = value;
        }

        public IJsValue Evaluate(IEnvironment environment)
        {
            return Operator.Evaluate(environment, Value.Evaluate(environment));
        }
    }
}