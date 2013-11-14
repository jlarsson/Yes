using Yes.Runtime;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public interface IPropertyDescriptor: IReference
    {
        string Name { get; }
        bool Writable { get; }
        bool Enumerable { get; }
        bool Configurable { get; }
        IPropertyDescriptor MakeOwnCopy(IEnvironment environment, IJsValue value);
    }
}