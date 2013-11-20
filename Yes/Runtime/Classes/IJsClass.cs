using System.Collections.Generic;
using Yes.Interpreter.Model;

namespace Yes.Runtime.Classes
{
    public interface IJsClass
    {
        IJsObject Prototype { get; }
        IPropertyDescriptor GetInstanceProperty(string name);
        IEnumerable<IPropertyDescriptor> GetInstanceProperties();
    }
}