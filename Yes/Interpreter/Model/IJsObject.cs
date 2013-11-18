using System.Collections.Generic;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public interface IJsObject: IJsValue
    {
        bool Extensible { get; }
        IJsObject GetPrototype();
        IPropertyDescriptor GetOwnProperty(string name);
        IPropertyDescriptor GetProperty(string name);
        //IJsValue Get(string name);
        //bool CanPut(string name);
        //void Put(string name);
        bool HasProperty(string name);
        bool DeleteProperty(string name);
        //IPropertyDescriptor DefaultValue(string name);
        IEnumerable<IPropertyDescriptor> GetOwnProperties();
        IPropertyDescriptor DefineOwnProperty(IPropertyDescriptor descriptor);
        IJsValue CloneTo(IEnvironment environment);
    }
}