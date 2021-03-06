using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Runtime.Operators
{
    public interface IBinaryOperator
    {
        IJsValue Evaluate(IEnvironment environment, IJsValue lhs, IJsValue rhs);
    }
}