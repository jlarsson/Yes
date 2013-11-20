using System.Collections.Generic;
using Yes.Runtime.Environment;
using Yes.Runtime.Error;

namespace Yes.Interpreter.Model
{
    public interface IJsObject: IJsValue
    {
        bool Extensible { get; }
        IJsObject GetPrototype();
        IPropertyDescriptor GetOwnProperty(string name);
        IPropertyDescriptor GetProperty(string name);
        IEnumerable<IPropertyDescriptor> GetProperties();
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


    public static class JsValueExtensions
    {
        public static T Cast<T>(this IJsValue value) where T : class
        {
            var t = value as T;
            if (t == null)
            {
                throw new JsReferenceError();
            }
            return t;
        }
    }
}