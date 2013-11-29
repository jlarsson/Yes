using System;

namespace Yes.Interpreter.Model
{
    public class AccessorPropertyDescriptor: IPropertyDescriptor{
        private readonly Func<IJsValue, IJsValue> _getter;
        private readonly Func<IJsValue, IJsValue, IJsValue> _setter;

        public AccessorPropertyDescriptor(string name, Func<IJsValue, IJsValue> getter, Func<IJsValue, IJsValue, IJsValue> setter)
        {
            Name = name;
            _getter = getter;
            _setter = setter;
        }

        public IJsValue GetValue(IJsValue self)
        {
            return _getter(self);
        }

        public IJsValue SetValue(IJsValue self, IJsValue value)
        {
            return _setter(self, value);
        }

        public string Name { get; private set; }

        public bool Writable
        {
            get { return _setter != null; }
        }

        public bool Enumerable { get; set; }

        public bool Configurable { get; set; }
    }
}