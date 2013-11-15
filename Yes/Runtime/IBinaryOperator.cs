using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Runtime
{
    public interface IBinaryOperator
    {
        IJsValue Evaluate(IEnvironment environment, IJsValue lhs, IJsValue rhs);
    }
}