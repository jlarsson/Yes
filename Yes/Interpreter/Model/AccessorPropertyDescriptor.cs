using System;
using Yes.Runtime.Environment;

namespace Yes.Interpreter.Model
{
    public class AccessorPropertyDescriptor: IPropertyDescriptor{
        private readonly Func<IJsValue> _getter;
        private readonly Func<IJsValue, IJsValue> _setter;

        public AccessorPropertyDescriptor(string name, Func<IJsValue> getter, Func<IJsValue,IJsValue> setter)
        {
            Name = name;
            _getter = getter;
            _setter = setter;
        }

        public IJsValue GetValue()
        {
            return _getter();
        }

        public IJsValue SetValue(IJsValue value)
        {
            return _setter(value);
        }

        public string Name { get; private set; }

        public bool Writable
        {
            get { return _setter != null; }
        }

        public bool Enumerable { get; set; }

        public bool Configurable { get; set; }

        public IPropertyDescriptor MakeOwnCopy(IEnvironment environment, IJsValue value)
        {
            return new AccessorPropertyDescriptor(Name, _getter, _setter)
                       {
                           Enumerable = Enumerable,
                           Configurable = Configurable
                       };
        }
    }
}