namespace Yes.Interpreter.Model
{
    public interface IBooleanConstructor: IJsConstructor
    {
        IJsBool Construct(bool value);
    }
}