using Yes.Interpreter.Model;

namespace Yes.Interpreter.Ast
{
    public interface ILValue
    {
        IJsValue SetValue(IScope scope, IJsValue value);
    }
}