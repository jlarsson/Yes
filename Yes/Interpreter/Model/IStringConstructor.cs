namespace Yes.Interpreter.Model
{
    public interface IStringConstructor: IJsConstructor
    {
        IJsString Construct(string value);
    }
}