using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Runtime.Operators
{
    public interface IUnaryOperator
    {
        IJsValue Evaluate(IEnvironment environment, IJsValue value);
    }
}