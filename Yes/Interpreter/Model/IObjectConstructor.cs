using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public interface IObjectConstructor: IJsConstructor
    {
        IJsObject Construct(IEnvironment environment);
    }
}