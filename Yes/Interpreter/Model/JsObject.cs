using System;
using System.Collections.Generic;
using Yes.Runtime;
using Yes.Runtime.Environment;
using Yes.Utility;

namespace Yes.Interpreter.Model
{
    public class JsObject : IJsObject
    {
        private Dictionary<string, IPropertyDescriptor> _properties;
        public IEnvironment Environment { get; private set; }
        public IJsObject Prototype { get; protected set; }

        public JsObject(IEnvironment environment, IJsObject prototype)
        {
            Environment = environment;
            Prototype = prototype;
            Extensible = true;
            DefineOwnProperty(new AccessorPropertyDescriptor("prototype", () => Prototype,
                                                             v => Prototype = Conversion.Cast<IJsObject>(v, true)));
        }

        public virtual JsTypeCode TypeCode
        {
            get { return JsTypeCode.Object; }
        }

        public virtual IReference GetReference(IJsValue name)
        {
            return GetReference(name.ToString());
        }

        public virtual IReference GetReference(string name)
        {
            // TODO: By specifying wether reference is readonly or not, we can optimze
            // especially when property is inherited

            // Find property definition in inheritance chain
            // Equivalent of CanPut
            IJsObject proto = this;
            IPropertyDescriptor pd = null;
            while (proto != null)
            {
                pd = proto.GetOwnProperty(name);
                if (pd != null)
                {
                    if (!pd.Writable)
                    {
                        return new ReadonlyReference(pd);
                    }
                    break;
                }
                proto = proto.GetPrototype();
            }
            if (pd == null)
            {
                if (!Extensible)
                {
                    return new ReadonlyReference();
                }
                // Property not found, but we are extensible to create a new data property
                return _properties[name] = new DataPropertyDescriptor(name);
            }

            // If the property is not own, we make a copy on write that is own
            // This is equivalent of calling DefineOwnProperty
            if (!_properties.ContainsKey(name))
            {
                return new DefineOwnPropertyOnWriteReference(this, name, pd);
            }
            return pd;
        }

        public int? ToArrayIndex()
        {
            return null;
        }

        public bool ToBoolean()
        {
            return true;
        }

        public double ToNumber()
        {
            // TODO: Call ToNumber of primitive value
            return double.NaN;
        }

        public bool Extensible { get; set; }

        public IJsObject GetPrototype()
        {
            return Prototype;
        }

        public IPropertyDescriptor GetOwnProperty(string name)
        {
            IPropertyDescriptor pd;
            return ((_properties != null) && _properties.TryGetValue(name, out pd)) ? pd : null;
        }

        public IPropertyDescriptor DefineOwnProperty(IPropertyDescriptor descriptor)
        {
            if (_properties == null)
            {
                _properties = new Dictionary<string, IPropertyDescriptor>();
            }
            return _properties[descriptor.Name] = descriptor;
        }

        protected IJsValue SetOwnOrUpgradeInheritedProperty(string name, IJsValue value)
        {
            var pd = GetOwnProperty(name);
            if (pd != null)
            {
                return pd.SetValue(value);
            }

            if (!Extensible)
            {
                throw new ApplicationException();
            }

            var proto = GetPrototype();
            while (proto != null)
            {
                pd = GetOwnProperty(name);
                if (pd != null)
                {
                    DefineOwnProperty(pd.MakeOwnCopy(value));
                    return value;
                }
                proto = proto.GetPrototype();
            }
            throw new JsError();
        }

        protected class DefineOwnPropertyOnWriteReference : IReference
        {
            private readonly JsObject _owner;
            private readonly string _name;
            private readonly IPropertyDescriptor _inheritedPropertyDescriptor;

            public DefineOwnPropertyOnWriteReference(JsObject owner, string name,
                                                     IPropertyDescriptor inheritedPropertyDescriptor)
            {
                _owner = owner;
                _name = name;
                _inheritedPropertyDescriptor = inheritedPropertyDescriptor;
            }

            public IJsValue GetValue()
            {
                return (_owner.GetOwnProperty(_name) ?? _inheritedPropertyDescriptor).GetValue();
            }

            public IJsValue SetValue(IJsValue value)
            {
                return _owner.SetOwnOrUpgradeInheritedProperty(_name, value);
            }
        }
    }
}