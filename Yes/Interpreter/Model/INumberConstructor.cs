namespace Yes.Interpreter.Model
{
    public interface INumberConstructor: IJsConstructor
    {
        IJsNumber Construct(double value);
    }
}