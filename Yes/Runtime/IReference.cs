using Yes.Interpreter.Model;

namespace Yes.Runtime
{
    public interface IReference
    {
        IJsValue GetValue();
        IJsValue SetValue(IJsValue value);
    }
}