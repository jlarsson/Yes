using System;
using System.Collections.Generic;
using Yes.Interpreter.Model;

namespace Yes.Runtime.Classes
{
    public interface IReflectedPropertyDescriptors
    {
        Dictionary<string, IPropertyDescriptor> GetInstanceDescriptors(Type type);
    }
}
