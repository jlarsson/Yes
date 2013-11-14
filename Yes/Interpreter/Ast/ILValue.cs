using Yes.Interpreter.Model;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Ast
{
    public interface ILValue
    {
        IJsValue SetValue(IEnvironment environment, IJsValue value);
    }
}