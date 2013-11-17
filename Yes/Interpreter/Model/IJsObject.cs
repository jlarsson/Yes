using System.Collections.Generic;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public interface IJsObject: IJsValue
    {
        bool Extensible { get; }
        IJsObject GetPrototype();
        IPropertyDescriptor GetOwnProperty(string name);
        IEnumerable<IPropertyDescriptor> GetOwnProperties();
        IPropertyDescriptor DefineOwnProperty(IPropertyDescriptor descriptor);
        IJsValue CloneTo(IEnvironment environment);
    }
}