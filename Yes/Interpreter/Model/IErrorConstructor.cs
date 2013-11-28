namespace Yes.Interpreter.Model
{
    public interface IErrorConstructor: IJsConstructor
    {
        IJsObject Construct(string message, string fileName, int lineNumber);
    }
}