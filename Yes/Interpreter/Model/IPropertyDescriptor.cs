using Yes.Runtime;

namespace Yes.Interpreter.Model
{
    public interface IPropertyDescriptor: IReference
    {
        string Name { get; }
        bool Writable { get; }
        bool Enumerable { get; }
        bool Configurable { get; }
        IPropertyDescriptor MakeOwnCopy(IJsValue value);
    }
}