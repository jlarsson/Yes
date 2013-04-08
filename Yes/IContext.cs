using Yes.Interpreter;

namespace Yes
{
    public interface IContext
    {
        IScope Scope { get; }
    }
}