using System;
using Yes.Runtime;

namespace Yes.Interpreter.Model
{
    public class LambdaReference : IReference
    {
        private readonly string _name;
        private readonly Func<string, IJsValue> _getter;
        private readonly Func<string, IJsValue, IJsValue> _setter;

        public LambdaReference(string name, Func<string,IJsValue> getter, Func<string, IJsValue, IJsValue> setter)
        {
            _name = name;
            _getter = getter;
            _setter = setter;
        }

        public IJsValue GetValue()
        {
            return _getter(_name);
        }

        public IJsValue SetValue(IJsValue value)
        {
            return _setter(_name, value);
        }
    }
}