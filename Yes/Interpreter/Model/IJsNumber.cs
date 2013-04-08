namespace Yes.Interpreter.Model
{
    public interface IJsNumber : IJsValue
    {
        double Value { get; }
    }
}