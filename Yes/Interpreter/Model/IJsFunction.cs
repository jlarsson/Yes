namespace Yes.Interpreter.Model
{
    public interface IJsFunction : IJsValue
    {
        IJsValue Apply(IJsValue @this, params IJsValue[] arguments);
    }
}