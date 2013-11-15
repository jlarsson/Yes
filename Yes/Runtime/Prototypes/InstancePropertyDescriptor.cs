using System;
using System.Reflection;
using Yes.Interpreter.Model;

namespace Yes.Runtime.Prototypes
{
    public class InstancePropertyDescriptor: IPropertyDescriptor
    {
        private readonly MethodInfo _getter;
        private readonly MethodInfo _setter;

        public InstancePropertyDescriptor(string name, MethodInfo getter, MethodInfo setter)
        {
            Name = name;
            _getter = getter;
            _setter = setter;
        }

        public IJsValue GetValue(IJsValue self = null)
        {
            if (_getter == null)
            {
                throw new JsError();
            }
            return (_getter.Invoke(self, new object[0]) as IJsValue) ?? JsUndefined.Instance;
        }

        public IJsValue SetValue(IJsValue self, IJsValue value)
        {
            if (_setter == null)
            {
                throw new JsError();
            }
            return (_getter.Invoke(self, new object[]{value}) as IJsValue) ?? JsUndefined.Instance;
        }

        public string Name { get; private set; }

        public bool Writable { get { return _setter != null; } }
        public bool Enumerable { get; set; }
        public bool Configurable { get; set; }

        public IPropertyDescriptor MakeOwnCopy(IJsValue value)
        {
            throw new NotImplementedException();
        }
    }
}